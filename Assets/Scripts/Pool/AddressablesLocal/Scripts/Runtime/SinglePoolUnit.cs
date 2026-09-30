using System.Collections.Generic;
using Game.Pool.AddressablesLocal.Scripts.VO;
using Game.Shared.AddressablesLocal.Scripts.Utilities;
using UnityEngine;

namespace Game.Pool.AddressablesLocal.Scripts.Runtime
{
    public class SinglePoolUnit : IPool
    {
        private readonly Stack<IPoolObject> _objectStack;
        private readonly PoolConfig _config;
        private readonly IPoolCollection _rootPoolCollection;
        private readonly Transform _root;

        private HashSet<IPoolObject> _borrowedObjects;

        public SinglePoolUnit(PoolConfig config, IPoolCollection rootPoolCollection, Transform root)
        {
            _rootPoolCollection = rootPoolCollection;
            _config = config;
            _root = root;

            _borrowedObjects = new HashSet<IPoolObject>();

            var defaultCapacity = config.DefaultCapacity;

            _objectStack = new Stack<IPoolObject>(defaultCapacity);

            for (int i = 0; i < defaultCapacity; i++)
            {
                _objectStack.Push(CreateNew());
            }
        }

        public IPoolObject Get()
        {
            var poolObj = _objectStack.Count > 0 ? _objectStack.Pop() : CreateNew();

            poolObj.Go.transform.SetParent(null);
            poolObj.GetFromPool();

            _borrowedObjects.Add(poolObj);
            
            return poolObj;
        }

        public T Get<T>() where T : IPoolObject
        {
            return (T)Get();
        }

        public void Return(IPoolObject poolObject)
        {
            if (poolObject.Pool != this)
            {
                "something wrong here!".Print();
                return;
            }

            _borrowedObjects.Remove(poolObject);
            
            poolObject.ReturnedToPool();
            poolObject.Go.transform.SetParent(_root, false);
            _objectStack.Push(poolObject);
        }

        public void ReturnAll()
        {
            var borrowedObjects = new List<IPoolObject>(_borrowedObjects);
            
            foreach (var borrowedObject in borrowedObjects)
                Return(borrowedObject);
        }

        private IPoolObject CreateNew()
        {
            var newPoolObj = _config.Creator.CreatePoolBehaviour(_config, this, _root);
            newPoolObj.Pool = this;
            newPoolObj.Key = _config.PoolKey;
            return newPoolObj;
        }

        public void Release()
        {
            if (_config.Releaser == null)
                return;

            foreach (var poolObject in _objectStack)
                _config.Releaser.Release(poolObject);
        }
    }
}