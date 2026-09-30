using UnityEngine;

namespace Game.Sorcerum
{
    [CreateAssetMenu(menuName = "ScriptableObject/Ball Data", fileName = "BallData", order = 0)]
    public class BallDataSo : ScriptableObject
    {
        [SerializeField] private BrainSo _brain;
        [SerializeField] private PoolKey _poolKey;

        public PoolKey PoolKey => _poolKey;

        public BrainSo Brain => _brain;

    }
}