using System.Collections.Generic;
using UnityEngine;

namespace Game.Sorcerum
{
    [CreateAssetMenu(menuName = "ScriptableData/Brain Data", fileName = "BrainData", order = 0)]
    public class BrainSo : ScriptableObject
    {
        [SerializeReference,SubclassSelector] private List<AttributeBase> _attributeBaseList = new();

        public IReadOnlyList<AttributeBase> AttributeBaseList => _attributeBaseList;
    }
}