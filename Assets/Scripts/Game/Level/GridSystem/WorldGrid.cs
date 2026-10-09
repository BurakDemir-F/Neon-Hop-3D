using System.Collections.Generic;
using General.GridSystem;
using UnityEngine;

namespace Game.Sorcerum
{
    public class WorldGrid : MonoBehaviour
    {
        [Tooltip("Bottom Left Of Grid"), SerializeField] 
        private Transform _gridOrigin;

        [Header("Grid Centering")]
        [Tooltip("World position representing the center of the screen where the grid should be centered.")]
        [SerializeField] private Vector3 _targetGridCenter = new Vector3(0f, 0f, 2f);
        
        private IGrid<MapGridCell> _grid;
        private GridPositionProvider _positionProvider;
        
        public Transform GridOrigin => _gridOrigin;
        
        public Vector3 TargetGridCenter
        {
            get => _targetGridCenter;
            set => _targetGridCenter = value;
        }

        public void PositionGridOrigin(IGridData gridData, Vector3? targetCenter = null)
        {
            if (_gridOrigin == null || gridData == null || gridData.Dimensions.x <= 0 || gridData.Dimensions.y <= 0)
                return;

            var center = targetCenter ?? _targetGridCenter;

            float step = gridData.CellSize + gridData.Padding;
            float totalWidth = (gridData.Dimensions.x - 1) * step + gridData.CellSize;
            float totalDepth = (gridData.Dimensions.y - 1) * step + gridData.CellSize;

            float originX = center.x - (totalWidth * 0.5f);
            float originZ = center.z - (totalDepth * 0.5f);
            float originY = center.y;

            _gridOrigin.position = new Vector3(originX, originY, originZ);
        }

        public void Initialize(IReadOnlyList<MapGridCell> cells, IGridData gridData)
        {
            PositionGridOrigin(gridData);
            _grid = new Grid<MapGridCell>(cells, gridData.Dimensions.x, gridData.Dimensions.y);
            _positionProvider = new BottomLeftGridPositionProvider(_gridOrigin, gridData.CellSize, gridData.Padding);
            
            for (var i = 0; i < cells.Count; i++)
            {
                var mapGridCell = cells[i];
                
                mapGridCell.WorldPos = _positionProvider.GetWorldPosition(new Vector2Int(mapGridCell.XPos,
                    mapGridCell.YPos));
                
                mapGridCell.Go.transform.SetParent(_gridOrigin);
            }
        }

        public void CalculateWorldPos(float height)
        {
            if (_gridOrigin == null || _grid == null || _positionProvider == null)
                return;

            var currentPos = _gridOrigin.position;
            _gridOrigin.position = new Vector3(currentPos.x, height, currentPos.z);

            foreach (var mapGridCell in _grid)
            {
                mapGridCell.WorldPos = _positionProvider.GetWorldPosition(new Vector2Int(mapGridCell.XPos,
                    mapGridCell.YPos));
            }
        }

        public Vector3 GetCellWorldPos(Vector2Int gridCellPosition)
        {
            return _positionProvider != null ? _positionProvider.GetWorldPosition(gridCellPosition) : Vector3.zero;
        }
        
        public MapGridCell GetCell(Vector2Int gridCellPosition)
        {
            return _grid != null ? _grid[gridCellPosition.x, gridCellPosition.y] : null;
        }

        public void ClearWorldGrid()
        {
            if (_grid == null)
                return;

            foreach (var mapGridCell in _grid)
            {
                mapGridCell.ClearCell();
                mapGridCell.ReturnToPool();
            }

            _grid = null;
            _positionProvider = null;
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(_targetGridCenter, 0.3f);
        }
#endif
    }
}