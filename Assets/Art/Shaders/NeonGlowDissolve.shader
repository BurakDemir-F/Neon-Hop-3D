Shader "Custom/NeonGlowDissolve"
{
    Properties
    {
        [MainColor] _BaseColor ("Base Color", Color) = (0.0, 0.85, 1.0, 1.0)
        _Color ("Color (Alias)", Color) = (0.0, 0.85, 1.0, 1.0)
        _CoreDarkness ("Core Darkness", Range(0.0, 1.0)) = 0.25
        
        [Header(Neon Edge Glow)]
        _RimColor ("Rim Color (Alpha=0 uses BaseColor)", Color) = (0, 0, 0, 0)
        _RimPower ("Rim Power", Range(0.5, 8.0)) = 2.6
        _RimIntensity ("Rim Intensity", Range(0.0, 15.0)) = 4.2
        _EmissionBoost ("Emission Multiplier", Range(0.5, 5.0)) = 1.8
        
        [Header(Dissolve)]
        _DissolveAmount ("Dissolve Amount", Range(0.0, 1.0)) = 0.0
        _DissolveWidth ("Dissolve Edge Width", Range(0.01, 0.3)) = 0.08
        [HDR] _DissolveEdgeColor ("Dissolve Edge Color (Alpha=0 uses BaseColor)", Color) = (0, 0, 0, 0)
        _DissolveIntensity ("Dissolve Edge Glow", Range(1.0, 25.0)) = 8.0
        _NoiseScale ("Noise Scale", Float) = 4.5
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
                float3 positionOS   : TEXCOORD2;
                float2 uv           : TEXCOORD3;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _Color;
                float4 _RimColor;
                float4 _DissolveEdgeColor;
                float _CoreDarkness;
                float _RimPower;
                float _RimIntensity;
                float _EmissionBoost;
                float _DissolveAmount;
                float _DissolveWidth;
                float _DissolveIntensity;
                float _NoiseScale;
            CBUFFER_END

            float Hash31(float3 p)
            {
                p = frac(p * float3(0.1031, 0.1030, 0.0973));
                p += dot(p, p.yzx + 33.33);
                return frac((p.x + p.y) * p.z);
            }

            float ValueNoise3D(float3 p)
            {
                float3 i = floor(p);
                float3 f = frac(p);
                float3 u = f * f * (3.0 - 2.0 * f);

                float n000 = Hash31(i + float3(0.0, 0.0, 0.0));
                float n100 = Hash31(i + float3(1.0, 0.0, 0.0));
                float n010 = Hash31(i + float3(0.0, 1.0, 0.0));
                float n110 = Hash31(i + float3(1.0, 1.0, 0.0));
                float n001 = Hash31(i + float3(0.0, 0.0, 1.0));
                float n101 = Hash31(i + float3(1.0, 0.0, 1.0));
                float n011 = Hash31(i + float3(0.0, 1.0, 1.0));
                float n111 = Hash31(i + float3(1.0, 1.0, 1.0));

                return lerp(
                    lerp(lerp(n000, n100, u.x), lerp(n010, n110, u.x), u.y),
                    lerp(lerp(n001, n101, u.x), lerp(n011, n111, u.x), u.y),
                    u.z
                );
            }

            float ProceduralFBM(float3 p)
            {
                float val = 0.0;
                float amp = 0.55;
                for (int i = 0; i < 3; i++)
                {
                    val += amp * ValueNoise3D(p);
                    p *= 2.08;
                    amp *= 0.48;
                }
                return val;
            }

            Varyings vert(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);

                output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.positionCS = TransformWorldToHClip(output.positionWS);
                output.positionOS = input.positionOS.xyz;
                output.uv = input.uv;
                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);

                // Procedural Dissolve
                float noiseVal = ProceduralFBM(input.positionOS * _NoiseScale);
                float dissolveAmt = _DissolveAmount;

                float3 rimCol = (_RimColor.a > 0.05) ? _RimColor.rgb : _BaseColor.rgb;

                float3 dissolveGlow = float3(0, 0, 0);
                if (dissolveAmt > 0.001)
                {
                    clip(noiseVal - dissolveAmt);

                    float edgeDist = noiseVal - dissolveAmt;
                    float edgeWidth = max(_DissolveWidth, 0.001);
                    if (edgeDist < edgeWidth)
                    {
                        float edgeFactor = saturate(1.0 - (edgeDist / edgeWidth));
                        float3 burnCol = (_DissolveEdgeColor.a > 0.05) ? _DissolveEdgeColor.rgb : (rimCol * 3.0);
                        dissolveGlow = burnCol * (pow(edgeFactor, 1.6) * _DissolveIntensity);
                    }
                }

                // Normal & View Direction
                float3 N = normalize(input.normalWS);
                float3 V = normalize(_WorldSpaceCameraPos.xyz - input.positionWS);
                float NdotV = saturate(dot(N, V));

                // Fresnel Neon Edge Glow
                float rim = pow(1.0 - NdotV, _RimPower);
                float3 neonEdge = rimCol * (rim * _RimIntensity * _EmissionBoost);

                // Soft Lighting & Core Color
                Light mainLight = GetMainLight();
                float NdotL = saturate(dot(N, mainLight.direction));
                float3 diffuse = mainLight.color * (NdotL * 0.35 + 0.65);
                float3 coreColor = _BaseColor.rgb * _CoreDarkness * diffuse;

                // Subtle Specular
                float3 H = normalize(mainLight.direction + V);
                float spec = pow(saturate(dot(N, H)), 24.0) * 0.35;
                float3 specular = mainLight.color * spec;

                float3 finalColor = coreColor + specular + neonEdge + dissolveGlow;
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

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _Color;
                float4 _RimColor;
                float4 _DissolveEdgeColor;
                float _CoreDarkness;
                float _RimPower;
                float _RimIntensity;
                float _EmissionBoost;
                float _DissolveAmount;
                float _DissolveWidth;
                float _DissolveIntensity;
                float _NoiseScale;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float3 positionOS   : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            float Hash31(float3 p)
            {
                p = frac(p * float3(0.1031, 0.1030, 0.0973));
                p += dot(p, p.yzx + 33.33);
                return frac((p.x + p.y) * p.z);
            }

            float ValueNoise3D(float3 p)
            {
                float3 i = floor(p);
                float3 f = frac(p);
                float3 u = f * f * (3.0 - 2.0 * f);

                float n000 = Hash31(i + float3(0.0, 0.0, 0.0));
                float n100 = Hash31(i + float3(1.0, 0.0, 0.0));
                float n010 = Hash31(i + float3(0.0, 1.0, 0.0));
                float n110 = Hash31(i + float3(1.0, 1.0, 0.0));
                float n001 = Hash31(i + float3(0.0, 0.0, 1.0));
                float n101 = Hash31(i + float3(1.0, 0.0, 1.0));
                float n011 = Hash31(i + float3(0.0, 1.0, 1.0));
                float n111 = Hash31(i + float3(1.0, 1.0, 1.0));

                return lerp(
                    lerp(lerp(n000, n100, u.x), lerp(n010, n110, u.x), u.y),
                    lerp(lerp(n001, n101, u.x), lerp(n011, n111, u.x), u.y),
                    u.z
                );
            }

            float ProceduralFBM(float3 p)
            {
                float val = 0.0;
                float amp = 0.55;
                for (int i = 0; i < 3; i++)
                {
                    val += amp * ValueNoise3D(p);
                    p *= 2.08;
                    amp *= 0.48;
                }
                return val;
            }

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
                output.positionOS = input.positionOS.xyz;
                return output;
            }

            half4 ShadowPassFragment(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);

                if (_DissolveAmount > 0.001)
                {
                    float noiseVal = ProceduralFBM(input.positionOS * _NoiseScale);
                    clip(noiseVal - _DissolveAmount);
                }

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

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _Color;
                float4 _RimColor;
                float4 _DissolveEdgeColor;
                float _CoreDarkness;
                float _RimPower;
                float _RimIntensity;
                float _EmissionBoost;
                float _DissolveAmount;
                float _DissolveWidth;
                float _DissolveIntensity;
                float _NoiseScale;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS   : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float3 positionOS   : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            float Hash31(float3 p)
            {
                p = frac(p * float3(0.1031, 0.1030, 0.0973));
                p += dot(p, p.yzx + 33.33);
                return frac((p.x + p.y) * p.z);
            }

            float ValueNoise3D(float3 p)
            {
                float3 i = floor(p);
                float3 f = frac(p);
                float3 u = f * f * (3.0 - 2.0 * f);

                float n000 = Hash31(i + float3(0.0, 0.0, 0.0));
                float n100 = Hash31(i + float3(1.0, 0.0, 0.0));
                float n010 = Hash31(i + float3(0.0, 1.0, 0.0));
                float n110 = Hash31(i + float3(1.0, 1.0, 0.0));
                float n001 = Hash31(i + float3(0.0, 0.0, 1.0));
                float n101 = Hash31(i + float3(1.0, 0.0, 1.0));
                float n011 = Hash31(i + float3(0.0, 1.0, 1.0));
                float n111 = Hash31(i + float3(1.0, 1.0, 1.0));

                return lerp(
                    lerp(lerp(n000, n100, u.x), lerp(n010, n110, u.x), u.y),
                    lerp(lerp(n001, n101, u.x), lerp(n011, n111, u.x), u.y),
                    u.z
                );
            }

            float ProceduralFBM(float3 p)
            {
                float val = 0.0;
                float amp = 0.55;
                for (int i = 0; i < 3; i++)
                {
                    val += amp * ValueNoise3D(p);
                    p *= 2.08;
                    amp *= 0.48;
                }
                return val;
            }

            Varyings DepthOnlyVertex(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);

                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.positionOS = input.positionOS.xyz;
                return output;
            }

            half4 DepthOnlyFragment(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);

                if (_DissolveAmount > 0.001)
                {
                    float noiseVal = ProceduralFBM(input.positionOS * _NoiseScale);
                    clip(noiseVal - _DissolveAmount);
                }

                return 0;
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}
