using System;
using UnityEngine;

namespace Game.Pool.AddressablesLocal.Scripts.Runtime
{
    public interface IPoolObject
    {
        string Key { get; set; }
        void GetFromPool();
        void ReturnedToPool();
        IPool Pool { get; set; }
        GameObject Go { get; }
        event Action OnGetFromPool;
        event Action OnReturnToPool;
        T As<T>() where T : class,IPoolObject;
    }
}