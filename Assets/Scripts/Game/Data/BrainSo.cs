using System.Collections.Generic;
using UnityEngine;

namespace Game.Sorcerum
{
    public class BrainSo : ScriptableObject
    {
        [SerializeReference, SubclassSelector] private List<AttributeBase> _attributeBaseList;

        public IReadOnlyList<AttributeBase> AttributeBaseList => _attributeBaseList;
    }
}