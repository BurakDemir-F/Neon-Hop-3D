using System.Collections.Generic;
using UnityEngine;

namespace General.GridSystem
{
    public interface IGrid<T> : IEnumerable<T> where T : IGridCell
    {
        int GetSize();
        Vector2Int GetDimensions();
        T GetLast();
        T GetFirst();
        T this[int x, int y] { get; set; }
        bool TryGetNextCell(T cell, Direction direction, out T nextCell);
        List<Neighbor<T>> GetNeighbors(T cell);
    }
}