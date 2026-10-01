using System.Collections.Generic;
using General.GridSystem;
using UnityEngine;

namespace Game.Sorcerum
{
    public class WorldGrid : MonoBehaviour
    {
        [Tooltip("Bottom Left Of Grid"),SerializeField] 
        private Transform _gridOrigin;
        
        private IGrid<MapGridCell> _grid;
        private GridPositionProvider _positionProvider;

        public Transform GridOrigin => _gridOrigin;
        
        public void Initialize(IReadOnlyList<MapGridCell> cells, IGridData gridData)
        {
            _grid = new Grid<MapGridCell>(cells, gridData.Dimensions.x, gridData.Dimensions.y);
            _positionProvider = new BottomLeftGridPositionProvider(_gridOrigin, gridData.CellSize, gridData.Padding);
            
            for (var i = 0; i < cells.Count; i++)
            {
                var mapGridCell = cells[i];
                
                mapGridCell.WorldPos = _positionProvider.GetWorldPosition(new Vector2Int(mapGridCell.XPos,
                    mapGridCell.YPos));
            }
        }

        public void CalculateWorldPos(float height)
        {
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
            return _positionProvider.GetWorldPosition(gridCellPosition);
        }
        
        public MapGridCell GetCell(Vector2Int gridCellPosition)
        {
            return _grid[gridCellPosition.x, gridCellPosition.y];
        }
    }
}