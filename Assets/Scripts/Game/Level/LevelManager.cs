using System.Collections.Generic;
using System.Linq;
using Game.Pool.AddressablesLocal.Scripts.Runtime;
using UnityEngine;

namespace Game.Sorcerum
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private WorldGrid _worldGrid;
        
        [Header("Test")]
        [SerializeField] private LevelDataSo _testLevel;

        [SerializeField] private MasterPool _masterPool;
        
        private IPoolCollection _poolCollection;
        public void BuildLevel(LevelDataSo levelData)
        {
            CreateGrid(levelData);
            CreateStacks(levelData);
            CreateAvailableBallArea(levelData);
        }

        [ContextMenu("Build Level")]
        private void BuildLevelTest()
        {
            _masterPool.CheckAndInitialize(_worldGrid.GridOrigin);
            _poolCollection = _masterPool;
            BuildLevel(_testLevel);
        }

        [ContextMenu("Clear text objects")]
        private void ClearTestObjects()
        {
            while (_worldGrid.GridOrigin.childCount > 0)
            {
                DestroyImmediate(_worldGrid.GridOrigin.GetChild(0).gameObject);
            }
        }

        private void CreateAvailableBallArea(LevelDataSo levelData)
        {
            
        }

        private void CreateStacks(LevelDataSo levelData)
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
                    stack.AddToStack(item);
                }
                
                mapCell.PlaceStackToCell(stack);
            }
        }

        private void CreateGrid(LevelDataSo levelDataSo)
        {
            var gridData = levelDataSo.LevelDataVo.MapGrid;
            var emptyCellKey = levelDataSo.LevelDataVo.EmptyGridCellKey;
            var stackCellKey = levelDataSo.LevelDataVo.StackGridCellKey;

            var stackCellPositionList = levelDataSo.LevelDataVo.StackDataList.Select(stackData => stackData.GridPos);

            var positionSet = new HashSet<Vector2Int>(stackCellPositionList);

            var gridDataDimensions = gridData.Dimensions;

            var cellList = new List<MapGridCell>();
            
            for (int x = 0; x < gridDataDimensions.x; x++)
            {
                for (int y = 0; y < gridDataDimensions.y; y++)
                {
                    if (positionSet.Contains(new Vector2Int(x, y)))
                    {
                        var stackCell = _poolCollection.Get<MapGridCell>(stackCellKey.PoolKey1);
                        cellList.Add(stackCell);
                    }
                    else
                    {
                        var emptyCell = _poolCollection.Get<EmptyGridCell>(emptyCellKey.PoolKey1);
                        cellList.Add(emptyCell);
                    }
                }
            }
            
    
            _worldGrid.Initialize(cellList, levelDataSo.LevelDataVo.MapGrid);
            _worldGrid.CalculateWorldPos(0f);
        }
    }
    
}