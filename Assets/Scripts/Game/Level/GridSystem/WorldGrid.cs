using System.Collections.Generic;
using General.GridSystem;
using UnityEngine;
using Grid = General.GridSystem.Grid;

namespace Game.Sorcerum
{
    public class WorldGrid : MonoBehaviour
    {
        [Tooltip("Bottom Left Of Grid"),SerializeField] 
        private Transform _gridOrigin;
        
        private IGrid _grid;
        private GridPositionProvider _positionProvider;

        public WorldGrid(IReadOnlyList<IGridCell> cells, IGridData gridData)
        {
            _grid = new Grid(cells, gridData.Dimensions.x, gridData.Dimensions.y);
            _positionProvider = new BottomLeftGridPositionProvider(_gridOrigin, gridData.CellSize, gridData.Padding);
        }

        public Vector3 GetCellWorldPos(Vector2Int gridCellPosition)
        {
            return _positionProvider.GetWorldPosition(gridCellPosition);
        }
    }
}