using UnityEngine;

namespace Game.Sorcerum
{
    [CreateAssetMenu(menuName = "ScriptableData/Stackable Item", fileName = "Stackable Item", order = 0)]
    public class StackableItemDataSo : ScriptableObject
    {
        [SerializeField] private BrainSo _brain;
        [SerializeField] private PoolKey _poolKey;

        public BrainSo Brain => _brain;
        

        public PoolKey PoolKey => _poolKey;
    }
}