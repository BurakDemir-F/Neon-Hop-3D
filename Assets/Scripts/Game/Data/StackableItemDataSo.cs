using UnityEngine;

namespace Game.Sorcerum
{
    public class StackableItemDataSo : ScriptableObject
    {
        [SerializeField] private Vector2Int _gridPos;
        [SerializeField] private BrainSo _brain;
        [SerializeField] private PoolKey _poolKey;

        public BrainSo Brain => _brain;
        

        public PoolKey PoolKey => _poolKey;

        public Vector2Int GridPos
        {
            get => _gridPos;
            set => _gridPos = value;
        }
    }
}