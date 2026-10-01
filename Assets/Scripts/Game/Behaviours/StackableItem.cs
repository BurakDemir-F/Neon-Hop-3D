using System;
using Game.Pool.AddressablesLocal.Scripts.Runtime;
using General;
using UnityEngine;

namespace Game.Sorcerum
{
    public class StackableItem : MonoBehaviour, IPoolObject
    {
        [SerializeField] private StackableItemDataSo _itemData;
        
        private AttributeCollection _attributeCollection;
        public Transform Transform => transform;

        public void Initialize(IContextProvider contextProvider)
        {
            _attributeCollection.Clear();
            
            foreach (var attributeBase in _itemData.Brain.AttributeBaseList)
                _attributeCollection.UpdateObject(attributeBase);
            
            foreach (var attributeBase in _attributeCollection)
                attributeBase.Initialize(contextProvider);
        }

        public void Crack()
        {
            var hasCrackAttribute = _attributeCollection.TryGetObject<CrackAttribute>(out var crackAttribute);
            
            if(!hasCrackAttribute)
                return;
        }
        
        public string Key { get; set; }
        public void GetFromPool()
        {
            gameObject.SetActive(true);
        }

        public void ReturnedToPool()
        {
            gameObject.SetActive(false);
        }

        public IPool Pool { get; set; }
        public GameObject Go => gameObject;
        public event Action OnGetFromPool;
        public event Action OnReturnToPool;
        public T As<T>() where T : class, IPoolObject
        {
            return this as T;
        }
    }
}