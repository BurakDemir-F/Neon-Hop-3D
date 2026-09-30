using UnityEngine;

namespace Game.Pool.AddressablesLocal.Scripts.Runtime
{
    
    public abstract class PoolObjectReleaser : ScriptableObject
    {
        public abstract void Release(IPoolObject poolObject);
    }
}