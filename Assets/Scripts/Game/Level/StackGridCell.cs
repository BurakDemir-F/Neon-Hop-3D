using System;
using System.Collections.Generic;
using Game.Pool.AddressablesLocal.Scripts.Runtime;
using UnityEngine;

namespace Game.Sorcerum
{
    public class StackGridCell : MapGridCell
    {
        [SerializeField] private Transform _cellOrigin;

        private IItemStack _stack;

        public void PlaceStackToCell(IItemStack stack)
        {
            _stack = stack;
            
            _stack.StackRoot.SetParent(_cellOrigin);
            _stack.StackRoot.localPosition = Vector3.zero;
        }

        public override void ClearCell()
        {
            _stack.ClearStack();
            _stack.ReturnToPool();
        }
    }
}