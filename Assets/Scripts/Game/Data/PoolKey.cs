using UnityEngine;

namespace Game.Sorcerum
{
    [System.Serializable]
    public class PoolKey
    {
        [SerializeField] private string _poolKey;

        public PoolKey() { }
        public PoolKey(string key) => _poolKey = key;

        public string PoolKey1 => _poolKey;
    }
}