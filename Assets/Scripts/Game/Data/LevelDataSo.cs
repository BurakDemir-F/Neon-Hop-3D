using System.Collections.Generic;
using UnityEngine;

namespace Game.Sorcerum
{
    [CreateAssetMenu(menuName = "SerializableData/LevelData", fileName = "LevelData", order = 0)]
    public class LevelDataSo : ScriptableObject
    {
        [SerializeField] private LevelDataVo _levelDataVo;

        public LevelDataVo LevelDataVo => _levelDataVo;
    }

    [System.Serializable]
    public class LevelDataVo
    {
        [SerializeField] public StackableItemStackData _stackData;
        [SerializeField] public AvailableBallData _ballData;

        public StackableItemStackData StackData => _stackData;

        public AvailableBallData BallData => _ballData;
    }

    public interface IGridData
    {
        Vector2Int Dimensions { get; }
        float CellSize { get; }
        float Padding { get; }
    }

    public class GridDataVo : IGridData
    {
        private Vector2Int _dimensions;
        private float _cellSize;
        private float _padding;

        public Vector2Int Dimensions => _dimensions;

        public float CellSize => _cellSize;

        public float Padding => _padding;
    }
    
    [System.Serializable]
    public class StackableItemStackData
    {
        [SerializeField] private Vector2Int _gridPos;
        [SerializeField] private List<StackableItemDataSo> _items;

        public Vector2Int GridPos => _gridPos;

        public List<StackableItemDataSo> Items => _items;
    }

    public class AvailableBallData
    {
        [SerializeField] private List<BallDataSo> _availableBalls;

        public List<BallDataSo> AvailableBalls => _availableBalls;
    }
}