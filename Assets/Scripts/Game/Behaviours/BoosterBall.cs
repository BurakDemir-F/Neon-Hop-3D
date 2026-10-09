using System.Collections;
using General;
using UnityEngine;

namespace Game.Sorcerum
{
    /// <summary>
    /// Specialized ball derivative with infinite toughness and particle trail.
    /// Traverses smoothly over all stacks, cracking top items regardless of color.
    /// </summary>
    public class BoosterBall : BallBase
    {
        [SerializeField] private ParticleSystem _trailParticle;

        private void Awake()
        {
            EnsureTrailParticle();
        }

        public void EnsureTrailParticle()
        {
            if (_trailParticle == null)
            {
                _trailParticle = GetComponentInChildren<ParticleSystem>();
            }

            if (_trailParticle == null)
            {
                var particleGo = new GameObject("TrailParticle");
                particleGo.transform.SetParent(transform, false);
                particleGo.transform.localPosition = Vector3.zero;

                _trailParticle = particleGo.AddComponent<ParticleSystem>();
                ConfigureParticleSystem(_trailParticle);
            }
        }

        private void ConfigureParticleSystem(ParticleSystem ps)
        {
            var main = ps.main;
            main.playOnAwake = true;
            main.loop = true;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.startLifetime = 0.35f;
            main.startSpeed = 0.5f;
            main.startSize = 0.35f;
            main.startColor = new ParticleSystem.MinMaxGradient(
                new Color(1f, 0.88f, 0.2f, 0.95f),
                new Color(1f, 0.45f, 0.05f, 0.8f)
            );

            var emission = ps.emission;
            emission.rateOverTime = 80f;

            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Sphere;
            shape.radius = 0.15f;

            var col = ps.colorOverLifetime;
            col.enabled = true;
            var grad = new Gradient();
            grad.SetKeys(
                new GradientColorKey[] {
                    new(new Color(1f, 0.88f, 0.2f), 0f),
                    new(new Color(1f, 0.35f, 0.05f), 1f)
                },
                new GradientAlphaKey[] {
                    new(1f, 0f),
                    new(0.85f, 0.3f),
                    new(0f, 1f)
                }
            );
            col.color = new ParticleSystem.MinMaxGradient(grad);

            var sol = ps.sizeOverLifetime;
            sol.enabled = true;
            var curve = new AnimationCurve();
            curve.AddKey(0f, 1f);
            curve.AddKey(1f, 0.1f);
            sol.size = new ParticleSystem.MinMaxCurve(1f, curve);

            var psr = ps.GetComponent<ParticleSystemRenderer>();
            if (psr != null)
            {
                var visualRenderer = GetComponentInChildren<MeshRenderer>();
                if (visualRenderer != null && visualRenderer.sharedMaterial != null)
                {
                    psr.sharedMaterial = visualRenderer.sharedMaterial;
                }
            }
        }

        public void InitializeBooster(IContextProvider contextProvider)
        {
            EnsureTrailParticle();

            // Infinite toughness / jump value
            RemainingJump = int.MaxValue;

            AttributeCollection.Clear();

            var boosterJump = new BoosterJumpAttribute();
            boosterJump.Initialize(contextProvider);
            AttributeCollection.UpdateObject<JumpAttribute>(boosterJump);
            AttributeCollection.UpdateObject(boosterJump);

            if (_trailParticle != null)
            {
                _trailParticle.Clear();
                _trailParticle.Play();
            }
        }

        public override IEnumerator Jump()
        {
            if (AttributeCollection.TryGetObject<BoosterJumpAttribute>(out var boosterAttribute))
            {
                yield return StartCoroutine(boosterAttribute.DoAttributeWork(this, new WorkChecker()));
            }
            else
            {
                yield return base.Jump();
            }
        }

        public override void GetFromPool()
        {
            base.GetFromPool();
            EnsureTrailParticle();
            if (_trailParticle != null)
            {
                _trailParticle.Clear();
                _trailParticle.Play();
            }
        }

        public override void ReturnedToPool()
        {
            if (_trailParticle != null)
            {
                _trailParticle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
            base.ReturnedToPool();
        }
    }
}
