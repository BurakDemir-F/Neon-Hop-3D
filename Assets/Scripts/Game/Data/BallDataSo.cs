using System.Collections.Generic;
using UnityEngine;

namespace Game.Sorcerum
{
    [CreateAssetMenu(menuName = "ScriptableData/Ball Data", fileName = "BallData", order = 0)]
    public class BallDataSo : ScriptableObject, IAttributeProvider
    {
        [SerializeField] private BrainSo _brain;
        [SerializeField] private PoolKey _poolKey;

        public PoolKey PoolKey => _poolKey;

        public BrainSo Brain => _brain;

        public bool TryGetAttribute<T>(out T attribute) where T : AttributeBase
        {
            if (_brain != null)
            {
                return _brain.TryGetAttribute(out attribute);
            }
            attribute = null;
            return false;
        }

        public IEnumerable<AttributeBase> GetAttributes()
        {
            return _brain.AttributeProvider.GetAttributes();
        }

        public T GetAttribute<T>() where T : AttributeBase
        {
            return _brain != null ? _brain.GetAttribute<T>() : null;
        }
    }
}