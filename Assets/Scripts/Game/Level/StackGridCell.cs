using System;
using Game.Pool.AddressablesLocal.Scripts.Runtime;
using UnityEngine;

namespace Game.Sorcerum
{
    public class StackGridCell : MapGridCell
    {
        [SerializeField] private Transform _cellOrigin;
        public void PlaceStackToCell(ItemStack stack)
        {
            stack.StackRoot.SetParent(_cellOrigin);
            stack.StackRoot.localPosition = Vector3.zero;
        }
        
    }
}