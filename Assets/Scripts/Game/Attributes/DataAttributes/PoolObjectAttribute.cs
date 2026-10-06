using UnityEngine;

namespace Game.Sorcerum
{
    [System.Serializable]
    public class PoolObjectAttribute : DataAttribute
    {
        [SerializeField] private PoolKey _poolKey;

        public PoolKey PoolKey => _poolKey;
    }
}