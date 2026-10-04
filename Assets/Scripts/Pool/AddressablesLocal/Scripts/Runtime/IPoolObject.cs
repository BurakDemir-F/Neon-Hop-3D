using System;
using UnityEngine;

namespace Game.Pool.AddressablesLocal.Scripts.Runtime
{
    public interface IPoolObject
    {
        GameObject Go { get; }
        T As<T>() where T : class,IPoolObject;
        void ReturnToPool();
    }

    public interface IPoolObjectSetter
    {
        IPool Pool { get; set; }
        string Key { get; set; }
        void GetFromPool();
        void ReturnedToPool();
    }
    public interface IPoolObjectController : IPoolObject, IPoolObjectSetter
    {
        
    }
    
    
}