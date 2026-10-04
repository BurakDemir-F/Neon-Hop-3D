using System.Collections.Generic;
using Game.Pool.AddressablesLocal.Scripts.SO;
using Game.Pool.AddressablesLocal.Scripts.VO;
using UnityEngine;

namespace Game.Pool.AddressablesLocal.Scripts.Runtime
{
    
    public class MasterPool : MonoBehaviour, IPoolCollection
    {
        [SerializeField] private PoolConfigSo _config;
        private Dictionary<string, IPool> _poolDict;
        [SerializeField]private bool _isInitialized;

        public void InitializePool(List<PoolConfig> config,Transform transform)
        {
            _poolDict = new Dictionary<string, IPool>();
            foreach (var poolConfig in config)
                _poolDict.Add(poolConfig.PoolKey, new SinglePoolUnit(poolConfig,transform));
        }

        public IPoolObject Get(string key)
        {
            return _poolDict[key].Get();
        }

        public T Get<T>(string key) where T : IPoolObject
        {
            return _poolDict[key].Get<T>();
        }

        public void Return(IPoolObject poolObj)
        {
            if (poolObj is IPoolObjectController controller)
            {
                _poolDict[controller.Key].Return(poolObj);
            }
        }

        public void ReturnAll()
        {
            foreach (var pool in _poolDict.Values)
                pool.ReturnAll();
        }

        public IPool GetPool(string key)
        {
            return _poolDict[key];
        }

        public void CheckAndInitialize(Transform root)
        {
            if (_isInitialized) return;
            
            InitializePool(_config.PoolConfig,root);
            _isInitialized = true;
        }

        public void Release()
        {
            foreach (var poolUnit in _poolDict.Values)
            {
                poolUnit.Release();
            }
        }
    }
}