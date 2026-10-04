using System;
using System.Collections.Generic;
using Game.Pool.AddressablesLocal.Scripts.Runtime;
using UnityEngine;

namespace Game.Sorcerum
{
    public class ItemStack : MonoBehaviour, IItemStack, IPoolObjectController
    {
        private StackableItemStackData _stackableItemStackData;
        [SerializeField] private Transform _stackRoot;
        private List<StackableItem> _itemList = new();

        public Transform StackRoot => _stackRoot;
        public Vector2Int GridPos => _stackableItemStackData.GridPos;

        public void Initialize(StackableItemStackData stackableItemStackData)
        {
            _stackableItemStackData = stackableItemStackData;
        }

        public void AddToStack(StackableItem stackableItem)
        {
            _itemList.Add(stackableItem);
            stackableItem.Transform.SetParent(_stackRoot);

            stackableItem.transform.localPosition = new Vector3(0f,
                _itemList.Count * _stackableItemStackData.ItemDistance,
                0f);
        }

        public void ClearStack()
        {
            foreach (var stackableItem in _itemList)
            {
                stackableItem.ReturnToPool();
            }
            
            _itemList.Clear();
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
        public bool GetTop(out StackableItem stackableItem)
        {
            var hasItem = _itemList.Count > 0;
            
            stackableItem = hasItem ? _itemList[^1] : null;

            return hasItem;
        }

        public string Key { get; set; }
        public void GetFromPool()
        {
            gameObject.SetActive(true);
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

    public interface IItemStack
    {
        void AddToStack(StackableItem stackableItem);
        StackableItem RemoveFromStack();
        bool HasItem();
        bool GetTop(out StackableItem stackableItem);
        public Transform StackRoot { get; }
        public void ClearStack();
        void ReturnToPool();
    }
}