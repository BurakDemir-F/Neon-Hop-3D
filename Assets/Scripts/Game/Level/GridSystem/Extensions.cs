using UnityEngine;

namespace General.GridSystem
{
    public static class Extensions
    {
        public static Vector2Int PositionOnGrid(this IGridCell cell) => new Vector2Int(cell.XPos, cell.YPos);
    }
}