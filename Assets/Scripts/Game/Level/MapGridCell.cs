using System;
using Game.Pool.AddressablesLocal.Scripts.Runtime;
using General.GridSystem;
using UnityEngine;

namespace Game.Sorcerum
{
    public class MapGridCell : MonoBehaviour, IPoolObjectController, IGridCell
    {
        public Vector3 WorldPos
        {
            get => transform.position;
            set => transform.position = value;
        }

        public virtual void ClearCell()
        {
            
        }
        string IPoolObjectSetter.Key { get; set; }

        void IPoolObjectSetter.GetFromPool()
        {
            gameObject.SetActive(true);
        }

        void IPoolObjectSetter.ReturnedToPool()
        {
            gameObject.SetActive(false);
        }

        IPool IPoolObjectSetter.Pool { get; set; }
        public GameObject Go => gameObject;

        public T As<T>() where T : class, IPoolObject
        {
            return this as T;
        }

        public void ReturnToPool()
        {
            ((IPoolObjectSetter)this).Pool.Return(this);
        }

        public int XPos { get; set; }
        public int YPos { get; set; }
    }
}