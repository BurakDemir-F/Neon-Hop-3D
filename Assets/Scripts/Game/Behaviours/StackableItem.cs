using General;
using UnityEngine;

namespace Game.Sorcerum
{
    public class StackableItem : MonoBehaviour
    {
        private AttributeCollection _attributeCollection;
        public Transform Transform => transform;

        public void Initialize(AttributeCollection collection, IContextProvider contextProvider)
        {
            _attributeCollection = collection;
            
            foreach (var attributeBase in _attributeCollection)
                attributeBase.Initialize(contextProvider);
        }

        public void Crack()
        {
            var hasCrackAttribute = _attributeCollection.TryGetObject<CrackAttribute>(out var crackAttribute);
            
            if(!hasCrackAttribute)
                return;
            
            crackAttribute.Crack();
        }
    }
}