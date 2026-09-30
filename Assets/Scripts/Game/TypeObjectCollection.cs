using System;
using System.Collections;
using System.Collections.Generic;

namespace General
{
    public class TypeObjectCollection<TObject> : IEnumerable<TObject>
    {
        private readonly Dictionary<Type, TObject> _typeObjectDict = new();
        public bool TryGetObject<T>(out T resultObj) where T : TObject
        {
            if (_typeObjectDict.TryGetValue(typeof(T), out var attributeBase))
            {
                resultObj = (T)attributeBase;
                return true;
            }

            resultObj = default;
            return false;
        }

        public void UpdateRange(IEnumerable<TObject> objectList)
        {
            foreach (var tObject in objectList)
            {
                UpdateObject(tObject);
            }
        }
        
        public void UpdateObject(TObject obj)
        {
            _typeObjectDict[obj.GetType()] = obj;
        }

        public void Clear()
        {
            _typeObjectDict.Clear();
        }

        public IEnumerator<TObject> GetEnumerator()
        {
            return _typeObjectDict.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}