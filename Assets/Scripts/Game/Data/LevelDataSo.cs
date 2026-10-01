using System.Collections.Generic;
using UnityEngine;

namespace Game.Sorcerum
{
    [CreateAssetMenu(menuName = "ScriptableData/LevelData", fileName = "LevelData", order = 0)]
    public class LevelDataSo : ScriptableObject
    {
        [SerializeField] private LevelDataVo _levelDataVo;

        public LevelDataVo LevelDataVo => _levelDataVo;
    }

    [System.Serializable]
    public class LevelDataVo
    {
        [SerializeField] private List<StackableItemStackData> _stackDataList;
        [SerializeField] private AvailableBallData _ballData;
        [SerializeField] private GridDataVo _mapGrid;

        [SerializeField] private PoolKey _stackPoolKey;
        [SerializeField] private PoolKey _emptyGridCellKey;
        [SerializeField] private PoolKey _stackGridCellKey;

        public PoolKey StackPoolKey => _stackPoolKey;

        public PoolKey StackGridCellKey => _stackGridCellKey;

        public PoolKey EmptyGridCellKey => _emptyGridCellKey;

        public GridDataVo MapGrid => _mapGrid;

        public List<StackableItemStackData> StackDataList => _stackDataList;

        public AvailableBallData BallData => _ballData;
    }

    public interface IGridData
    {
        Vector2Int Dimensions { get; }
        float CellSize { get; }
        float Padding { get; }
    }

    [System.Serializable]
    public class GridDataVo : IGridData
    {
        [SerializeField] private Vector2Int _dimensions;
        [SerializeField] private float _cellSize;
        [SerializeField] private float _padding;

        public Vector2Int Dimensions => _dimensions;

        public float CellSize => _cellSize;

        public float Padding => _padding;
    }
    
    [System.Serializable]
    public class StackableItemStackData
    {
        [SerializeField] private Vector2Int _gridPos;
        [SerializeField] private List<StackableItemDataSo> _items;
        [SerializeField] private float _itemDistance = 0.5f;

        public float ItemDistance => _itemDistance;

        public Vector2Int GridPos => _gridPos;

        public List<StackableItemDataSo> Items => _items;
    }

    [System.Serializable]
    public class AvailableBallData
    {
        [SerializeField] private List<BallDataSo> _availableBalls;

        public List<BallDataSo> AvailableBalls => _availableBalls;
    }
}