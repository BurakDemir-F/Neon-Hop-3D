using Game.Pool.AddressablesLocal.Scripts.VO;
using Game.Shared.AddressablesLocal.Scripts.Utilities;
using UnityEngine;

namespace Game.Pool.AddressablesLocal.Scripts.Runtime
{
    public abstract class PoolObjectCreator : ScriptableObject
    {
        public virtual IPoolObject CreatePoolBehaviour(PoolConfig config, IPool pool,Transform root)
        {
            var poolObj = Instantiate(config.PoolObjectPrefab,root);
            if (poolObj.TryGetComponent<IPoolObject>(out var component))
            {
                component.Pool = pool;
                component.Key = config.PoolKey;
                return component;
            }

            $"{config.PoolKey},prefab should implement IPoolObject with any component".Print();
            return default;
        }
    }
}