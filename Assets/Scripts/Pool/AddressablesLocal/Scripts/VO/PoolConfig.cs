using Game.Pool.AddressablesLocal.Scripts.Runtime;
using UnityEngine;

namespace Game.Pool.AddressablesLocal.Scripts.VO
{
    [System.Serializable]
    public class PoolConfig
    {
        public GameObject PoolObjectPrefab;
        public int DefaultCapacity;
        public string PoolKey;
        public PoolObjectCreator Creator;
        public PoolObjectReleaser Releaser;
    }
}