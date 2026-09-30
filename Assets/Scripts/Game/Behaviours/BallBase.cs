using System;
using Game.Pool.AddressablesLocal.Scripts.Runtime;
using General;
using UnityEngine;

namespace Game.Sorcerum
{
    public class BallBase : MonoBehaviour, IPoolObject
    {
        [SerializeField] private BallDataSo _ballData;
        
        private AttributeCollection _attributeCollection;

        public void Initialize(IContextProvider contextProvider)
        {
            foreach (var attributeBase in _attributeCollection)
                attributeBase.Initialize(contextProvider);
        }
        
        public void Jump(ItemStack from, ItemStack to)
        {
            var hasJumper = _attributeCollection.TryGetObject<JumpAttribute>(out var jumperAttribute);
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