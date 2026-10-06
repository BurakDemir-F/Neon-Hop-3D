using System;
using Game.Pool.AddressablesLocal.Scripts.Runtime;
using General;
using UnityEngine;

namespace Game.Sorcerum
{
    public class StackableItem : MonoBehaviour, IPoolObjectController
    {
        [SerializeField] private StackableItemDataSo _itemData;
        
        private AttributeCollection _attributeCollection = new();

        public AttributeCollection AttributeCollection => _attributeCollection;

        public Transform Transform => transform;
        public IItemStack CurrentStack { get; set; }

        public void Initialize(StackableItemDataSo itemData, IContextProvider contextProvider)
        {
            if (itemData != null)
            {
                _itemData = itemData;
            }

            Initialize(contextProvider);
        }

        public void Initialize(IContextProvider contextProvider)
        {
            _attributeCollection.Clear();

            if (_itemData != null && _itemData.Brain != null && _itemData.Brain.AttributeBaseList != null)
            {
                foreach (var attributeBase in _itemData.Brain.AttributeBaseList)
                    _attributeCollection.UpdateObject(attributeBase);
            }
            
            foreach (var attributeBase in _attributeCollection)
                attributeBase.Initialize(contextProvider);

            if (_attributeCollection.TryGetAttribute<ColorIdAttribute>(out var colorIdAttribute))
            {
                var renderer = GetComponentInChildren<MeshRenderer>();
                if (renderer != null)
                {
                    renderer.material.color = ColorIntConverter.IntToColor(colorIdAttribute.GetId());
                }
            }
        }

        public void Crack()
        {
            var hasCrackAttribute = _attributeCollection.TryGetObject<CrackAttribute>(out var crackAttribute);
            
            if(!hasCrackAttribute)
                return;

            DG.Tweening.ShortcutExtensions.DOScale(transform, Vector3.zero, 0.15f);
        }
        
        public string Key { get; set; }
        public void GetFromPool()
        {
            gameObject.SetActive(true);
            transform.localScale = Vector3.one;
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