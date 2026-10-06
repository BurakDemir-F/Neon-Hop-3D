using System.Collections.Generic;
using General;

namespace Game.Sorcerum
{
    public class AttributeCollection : TypeObjectCollection<AttributeBase>, IAttributeProvider
    {
        public AttributeCollection()
        {
        }

        public AttributeCollection(IEnumerable<AttributeBase> objectList) : base(objectList)
        {
        }

        public bool TryGetAttribute<T>(out T attribute) where T : AttributeBase
        {
            return TryGetObject<T>(out attribute);
        }

        public IEnumerable<AttributeBase> GetAttributes()
        {
            return this;
        }
    }

    public interface IAttributeProvider
    {
        bool TryGetAttribute<T>(out T attribute) where T : AttributeBase;
        IEnumerable<AttributeBase> GetAttributes();
    }
}