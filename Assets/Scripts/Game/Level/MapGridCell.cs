using System;
using Game.Pool.AddressablesLocal.Scripts.Runtime;
using General.GridSystem;
using UnityEngine;

namespace Game.Sorcerum
{
    public class MapGridCell : MonoBehaviour, IPoolObject, IGridCell
    {
        public Vector3 WorldPos
        {
            get => transform.position;
            set => transform.position = value;
        }
        
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

        public int XPos { get; set; }
        public int YPos { get; set; }
    }
}