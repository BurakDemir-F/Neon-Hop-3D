using System;
using System.Collections;
using Game.Pool.AddressablesLocal.Scripts.Runtime;
using General;
using UnityEngine;

namespace Game.Sorcerum
{
    public class BallBase : MonoBehaviour, IPoolObjectController
    {
        [SerializeField] private BallDataSo _ballData;

        private AttributeCollection _attributeCollection = new();
        public AttributeCollection AttributeCollection => _attributeCollection;

        public int RemainingJump { get; set; }

        public void Initialize(IAttributeProvider attributeProvider, IContextProvider contextProvider)
        {
            _attributeCollection.Clear();
            
            foreach (var attributeBase in attributeProvider.GetAttributes())
                _attributeCollection.UpdateObject(attributeBase);

            foreach (var attributeBase in _attributeCollection)
                attributeBase.Initialize(contextProvider);

            if (_attributeCollection.TryGetAttribute<ToughnessAttribute>(out var toughnessAttribute))
            {
                RemainingJump = toughnessAttribute.HitCount;
            }

            if (_attributeCollection.TryGetAttribute<ColorIdAttribute>(out var colorIdAttribute))
            {
                var renderer = GetComponentInChildren<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.material.color = ColorIntConverter.IntToColor(colorIdAttribute.GetId());
                }
            }
        }

        public void Initialize(IContextProvider contextProvider)
        {
            Initialize(_ballData, contextProvider);
        }

        public virtual IEnumerator Jump()
        {
            if (_attributeCollection.TryGetObject<JumpAttribute>(out var jumperAttribute))
            {
                yield return StartCoroutine(jumperAttribute.DoAttributeWork(this, new WorkChecker()));
            }

            yield break;
        }

        public string Key { get; set; }

        public virtual void GetFromPool()
        {
            gameObject.SetActive(true);
            transform.localScale = Vector3.one;
        }

        public virtual void ReturnedToPool()
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
        }

        void IPoolObjectSetter.ReturnedToPool()
        {
            ReturnedToPool();
        }

        public IPool Pool { get; set; }
        public GameObject Go => gameObject;

        public T As<T>() where T : class, IPoolObject
        {
            return this as T;
        }

        public void ReturnToPool()
        {
            Pool.Return(this);
        }
    }
}