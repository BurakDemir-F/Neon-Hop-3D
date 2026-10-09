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
            
            // Parent to this cell's root transform with uniform (1, 1, 1) scale.
            // Using _cellOrigin for local position offset so items align with the base platform.
            Stack.StackRoot.SetParent(transform, false);
            Stack.StackRoot.localPosition = _cellOrigin != null ? _cellOrigin.localPosition : Vector3.zero;
            Stack.StackRoot.localRotation = Quaternion.identity;
            Stack.StackRoot.localScale = Vector3.one;
        }

        public override void ClearCell()
        {
            if (Stack != null)
            {
                Stack.ClearStack();
                Stack.ReturnToPool();
                Stack = null;
            }
        }
    }
}