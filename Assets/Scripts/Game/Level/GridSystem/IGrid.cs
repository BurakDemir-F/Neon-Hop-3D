using System.Collections.Generic;
using UnityEngine;

namespace General.GridSystem
{
    public interface IGrid : IEnumerable<IGridCell>
    {
        int GetSize();
        Vector2Int GetDimensions();
        IGridCell GetLast();
        IGridCell GetFirst();
        IGridCell this[int x, int y] { get; set; }
        bool TryGetNextCell(IGridCell cell, Direction direction, out IGridCell nextCell);
        List<Neighbor> GetNeighbors(IGridCell cell);
    }
}