using System.Collections.Generic;
using UnityEngine;

namespace Game.Sorcerum
{
    public class ItemStack : MonoBehaviour
    {
        [SerializeField] private Transform _stackRoot;
        private List<StackableItem> _itemList = new();

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
    }
}