using System.Collections.Generic;
using System.Linq;
using Game.Pool.AddressablesLocal.Scripts.Runtime;
using General;
using UnityEngine;

namespace Game.Sorcerum
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private WorldGrid _worldGrid;
        
        [Header("Test")]
        [SerializeField] private LevelDataSo _testLevel;

        [SerializeField] private AvailableBallScreen _availableBallScreen;
        
        private IPoolCollection _poolCollection;
        public WorldData BuildLevel(LevelDataSo levelData, IPoolCollection poolCollection, IContextProvider contextProvider)
        {
            if (levelData == null)
            {
                Debug.LogWarning($"[{nameof(LevelManager)}] LevelData is null!", this);
                return null;
            }

            _poolCollection = poolCollection;
            
            if (_availableBallScreen == null)
            {
                _availableBallScreen = FindFirstObjectByType<AvailableBallScreen>();
            }

            ClearLevel();

            var worldData = CreateGrid(levelData);
            CreateStacks(levelData, contextProvider);
            CreateAvailableBallArea(levelData);

            return worldData;
        }

        public void ClearLevel()
        {
            _worldGrid.ClearWorldGrid();

            if (_availableBallScreen != null)
            {
                _availableBallScreen.ClearBalls();
            }
        }

        private void CreateAvailableBallArea(LevelDataSo levelData)
        {
            if (_availableBallScreen == null)
            {
                _availableBallScreen = FindFirstObjectByType<AvailableBallScreen>();
            }

            if (_availableBallScreen != null && levelData != null)
            {
                _availableBallScreen.ShowAvailableBalls(levelData.LevelDataVo.BallData);
            }
        }

        private void CreateStacks(LevelDataSo levelData, IContextProvider contextProvider)
        {
            var stackList = levelData.LevelDataVo.StackDataList;

            foreach (var stackData in stackList)
            {
                var gridPos = stackData.GridPos;
                var mapCell = _worldGrid.GetCell(gridPos) as StackGridCell;

                var stack = _poolCollection.Get<ItemStack>(levelData.LevelDataVo.StackPoolKey.PoolKey1);
                stack.Initialize(stackData);
                
                foreach (var stackableItemData in stackData.Items)
                {
                    var item = _poolCollection.Get<StackableItem>(stackableItemData.PoolKey.PoolKey1);
                    item.Initialize(contextProvider);
                    stack.AddToStack(item);
                }
                
                mapCell.PlaceStackToCell(stack);
            }
        }

        private WorldData CreateGrid(LevelDataSo levelDataSo)
        {
            var gridData = levelDataSo.LevelDataVo.MapGrid;
            var emptyCellKey = levelDataSo.LevelDataVo.EmptyGridCellKey;
            var stackCellKey = levelDataSo.LevelDataVo.StackGridCellKey;

            var stackCellPositionList = levelDataSo.LevelDataVo.StackDataList.Select(stackData => stackData.GridPos);

            var positionSet = new HashSet<Vector2Int>(stackCellPositionList);

            var gridDataDimensions = gridData.Dimensions;

            var cellList = new List<MapGridCell>(gridDataDimensions.x * gridDataDimensions.y);

            var stackCellList = new List<StackGridCell>();
            var emptyCellList = new List<EmptyGridCell>();
            
            for (int x = 0; x < gridDataDimensions.x; x++)
            {
                for (int y = 0; y < gridDataDimensions.y; y++)
                {
                    if (positionSet.Contains(new Vector2Int(x, y)))
                    {
                        var stackCell = _poolCollection.Get<StackGridCell>(stackCellKey.PoolKey1);
                        cellList.Add(stackCell);
                        stackCellList.Add(stackCell);
                    }
                    else
                    {
                        var emptyCell = _poolCollection.Get<EmptyGridCell>(emptyCellKey.PoolKey1);
                        cellList.Add(emptyCell);
                        emptyCellList.Add(emptyCell);
                    }
                }
            }
            
            _worldGrid.Initialize(cellList, levelDataSo.LevelDataVo.MapGrid);
            _worldGrid.CalculateWorldPos(0f);

            return new WorldData(stackCellList, emptyCellList);
        }
    }

    public class WorldData
    {
        public WorldData(IReadOnlyList<StackGridCell> stackCellList, IReadOnlyList<EmptyGridCell> emptyCellList)
        {
            StackCellList = stackCellList;
            EmptyCellList = emptyCellList;
        }

        public IReadOnlyList<StackGridCell> StackCellList { get; }
        public IReadOnlyList<EmptyGridCell> EmptyCellList { get; }
    }
    
}