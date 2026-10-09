Shader "Custom/NeonGridPlatform"
{
    Properties
    {
        [MainColor] _BaseColor ("Base Color", Color) = (0.04, 0.04, 0.08, 1.0)
        _Color ("Color (Alias)", Color) = (0.04, 0.04, 0.08, 1.0)
        
        [Header(Neon Edge Accent)]
        [HDR] _EdgeColor ("Edge Neon Color", Color) = (0.0, 0.85, 1.0, 1.0)
        _EdgeWidth ("Border Width", Range(0.01, 0.2)) = 0.06
        _EdgeIntensity ("Border Glow", Range(1.0, 15.0)) = 4.5
        
        [Header(Rim Glow)]
        _RimPower ("Rim Power", Range(0.5, 8.0)) = 2.8
        _RimIntensity ("Rim Glow", Range(0.0, 10.0)) = 2.5
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Opaque" 
            "RenderPipeline" = "UniversalPipeline" 
            "Queue" = "Geometry"
        }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            Cull Back
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float2 uv           : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float3 positionWS   : TEXCOORD0;
                float3 normalWS     : TEXCOORD1;
                float2 uv           : TEXCOORD2;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _Color;
                float4 _EdgeColor;
                float _EdgeWidth;
                float _EdgeIntensity;
                float _RimPower;
                float _RimIntensity;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);

                output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.positionCS = TransformWorldToHClip(output.positionWS);
                output.uv = input.uv;
                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);

                float3 N = normalize(input.normalWS);
                float3 V = normalize(_WorldSpaceCameraPos.xyz - input.positionWS);
                float NdotV = saturate(dot(N, V));

                // Soft Lighting
                Light mainLight = GetMainLight();
                float NdotL = saturate(dot(N, mainLight.direction));
                float3 diffuse = mainLight.color * (NdotL * 0.3 + 0.7);
                float3 baseLit = _BaseColor.rgb * diffuse;

                // Subtle Specular
                float3 H = normalize(mainLight.direction + V);
                float spec = pow(saturate(dot(N, H)), 32.0) * 0.25;
                float3 specular = mainLight.color * spec;

                // Fresnel Rim Glow
                float rim = pow(1.0 - NdotV, _RimPower);
                float3 sideRim = _EdgeColor.rgb * (rim * _RimIntensity);

                // Top Face Border Glow
                float topBorderGlow = 0.0;
                if (N.y > 0.5)
                {
                    float w = _EdgeWidth;
                    float borderX = step(w, input.uv.x) * step(input.uv.x, 1.0 - w);
                    float borderY = step(w, input.uv.y) * step(input.uv.y, 1.0 - w);
                    float isBorder = 1.0 - (borderX * borderY);

                    // Smooth edge glow
                    float distToEdgeX = min(input.uv.x, 1.0 - input.uv.x);
                    float distToEdgeY = min(input.uv.y, 1.0 - input.uv.y);
                    float minDist = min(distToEdgeX, distToEdgeY);
                    float glowFactor = saturate(1.0 - (minDist / w));
                    topBorderGlow = isBorder * pow(glowFactor, 1.2);
                }

                float3 topEdgeGlow = _EdgeColor.rgb * (topBorderGlow * _EdgeIntensity);

                float3 finalColor = baseLit + specular + sideRim + topEdgeGlow;
                return float4(finalColor, _BaseColor.a);
            }
            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull Back

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            float3 _LightDirection;
            float3 _LightPosition;

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            Varyings ShadowPassVertex(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);

                float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
                float3 normalWS = TransformObjectToWorldNormal(input.normalOS);

                #if _CASTING_PUNCTUAL_LIGHT_SHADOW
                    float3 lightDirectionWS = normalize(_LightPosition - positionWS);
                #else
                    float3 lightDirectionWS = _LightDirection;
                #endif

                output.positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));
                output.positionCS = ApplyShadowClamping(output.positionCS);
                return output;
            }

            half4 ShadowPassFragment(Varyings input) : SV_Target
            {
                return 0;
            }
            ENDHLSL
        }

        Pass
        {
            Name "DepthOnly"
            Tags { "LightMode" = "DepthOnly" }

            ZWrite On
            ColorMask R
            Cull Back

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex DepthOnlyVertex
            #pragma fragment DepthOnlyFragment
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            Varyings DepthOnlyVertex(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);

                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                return output;
            }

            half4 DepthOnlyFragment(Varyings input) : SV_Target
            {
                return 0;
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
