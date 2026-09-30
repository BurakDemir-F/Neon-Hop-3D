using General;
using UnityEngine;

namespace Game.Sorcerum
{
    public class BallBase : MonoBehaviour
    {
        private AttributeCollection _attributeCollection;

        public void Initialize(AttributeCollection collection, IContextProvider contextProvider)
        {
            _attributeCollection = collection;
            
            foreach (var attributeBase in _attributeCollection)
                attributeBase.Initialize(contextProvider);
        }
        
        public void Jump(ItemStack from, ItemStack to)
        {
            var hasJumper = _attributeCollection.TryGetObject<JumpAttribute>(out var jumperAttribute);
            
            if(hasJumper)
                jumperAttribute.Jump(from, to);
        }
    }
}