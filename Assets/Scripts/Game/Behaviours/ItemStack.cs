using System;
using System.Collections.Generic;
using Game.Pool.AddressablesLocal.Scripts.Runtime;
using UnityEngine;

namespace Game.Sorcerum
{
    public class ItemStack : MonoBehaviour, IItemStack, IPoolObject
    {
        private StackableItemStackData _stackableItemStackData;
        [SerializeField] private Transform _stackRoot;
        private List<StackableItem> _itemList = new();

        public void Initialize(StackableItemStackData stackableItemStackData)
        {
            _stackableItemStackData = stackableItemStackData;
        }

        public void AddToStack(StackableItem stackableItem)
        {
            _itemList.Add(stackableItem);
            stackableItem.Transform.SetParent(_stackRoot);
        }

        public StackableItem RemoveFromStack()
        {
            var index = _itemList.Count - 1;

            if (index < 0)
                return null;

            var item = _itemList[index];
            _itemList.RemoveAt(index);


            return item;
        }

        public bool HasItem() => _itemList.Count > 0;
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

    public interface IItemStack
    {
        void AddToStack(StackableItem stackableItem);
        StackableItem RemoveFromStack();
        bool HasItem();
    }
}