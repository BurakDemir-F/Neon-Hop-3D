using System;
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

        public void Initialize(IContextProvider contextProvider)
        {
            _attributeCollection.Clear();
            
            foreach (var attributeBase in _ballData.Brain.AttributeBaseList)
                _attributeCollection.UpdateObject(attributeBase);
            
            foreach (var attributeBase in _attributeCollection)
                attributeBase.Initialize(contextProvider);
            
            if (_attributeCollection.TryGetAttribute<ToughnessAttribute>(out var toughnessAttribute))
            {
                RemainingJump = toughnessAttribute.HitCount;
            }
        }
        
        public void Jump()
        {
            var hasJumper = _attributeCollection.TryGetObject<JumpAttribute>(out var jumperAttribute);
        }

        public string Key { get; set; }
        public void GetFromPool()
        {
            gameObject.SetActive(true);
        }

        void IPoolObjectSetter.ReturnedToPool()
        {
            gameObject.SetActive(false);
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