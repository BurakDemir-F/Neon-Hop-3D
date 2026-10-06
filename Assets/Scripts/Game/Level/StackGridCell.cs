using System;
using System.Collections.Generic;
using Game.Pool.AddressablesLocal.Scripts.Runtime;
using UnityEngine;

namespace Game.Sorcerum
{
    public class StackGridCell : MapGridCell
    {
        [SerializeField] private Transform _cellOrigin;

        public IItemStack Stack { get; private set; }

        public void PlaceStackToCell(IItemStack stack)
        {
            Stack = stack;
            
            Stack.StackRoot.SetParent(_cellOrigin);
            Stack.StackRoot.localPosition = Vector3.zero;
        }

        public override void ClearCell()
        {
            Stack.ClearStack();
            Stack.ReturnToPool();
        }
    }
}