using System.Collections.Generic;
using UnityEngine;

namespace Game.Sorcerum
{
    [CreateAssetMenu(menuName = "ScriptableData/Brain Data", fileName = "BrainData", order = 0)]
    public class BrainSo : ScriptableObject, IAttributeProvider
    {
        [SerializeReference,SubclassSelector] private List<AttributeBase> _attributeBaseList = new();

        public IReadOnlyList<AttributeBase> AttributeBaseList => _attributeBaseList;

        private IAttributeProvider _provider;
        public IAttributeProvider AttributeProvider => _provider ??= new AttributeCollection(_attributeBaseList);

        public bool TryGetAttribute<T>(out T attribute) where T : AttributeBase
        {
            return AttributeProvider.TryGetAttribute(out attribute);
        }

        public T GetAttribute<T>() where T : AttributeBase
        {
            TryGetAttribute<T>(out var attribute);
            return attribute;
        }
    }
}