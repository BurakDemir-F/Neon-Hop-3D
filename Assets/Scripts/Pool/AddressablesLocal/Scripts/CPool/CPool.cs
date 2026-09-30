using System.Collections.Generic;

namespace Game.Pool.AddressablesLocal.Scripts.CPool
{
    public class CPool<T> : ICPool<T> where T: ICPoolObject,new()
    {
        private Stack<T> _itemStack;

        public CPool(int initialAmount)
        {
            _itemStack = new Stack<T>();
            for (int i = 0; i < initialAmount; i++)
            {
                _itemStack.Push(CreateNew());
            }
        }
        public T Get()
        {
            if (_itemStack.Count > 0)
            {
                var item =  _itemStack.Pop();
                item.OnGet();
                return item;
            }

            var newItem = CreateNew();
            newItem.OnGet();
            return newItem;
        }

        public void Return(T poolObject)
        {
            poolObject.OnReturnedToPool();
            _itemStack.Push(poolObject);
        }

        private T CreateNew()
        {
            return new T();
        }
    }
}