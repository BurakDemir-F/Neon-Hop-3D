using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General.GridSystem
{
    public class Grid : IGrid
    {
        protected IGridCell[,] _items;
        protected int _xDimension;
        protected int _yDimension;
        
        public Grid(IReadOnlyList<IGridCell> cells, int xDimension,int yDimension)
        {
            _xDimension = xDimension;
            _yDimension = yDimension;

            _items = new IGridCell[_xDimension, _yDimension];

            for (int i = 0; i < xDimension; i++)
            {
                for (int j = 0; j < yDimension; j++)
                {
                    var cell = cells[GetIndex(_xDimension, i, j)];
                    _items[i, j] = cell;
                    cell.XPos = i;
                    cell.YPos = j;
                }
            }
        }

        private int GetIndex(int xDimension, int xPos, int yPos)
        {
            return xDimension * yPos + xPos;
        }
        
        public void SetCell(IGridCell cell)
        {
            _items[cell.XPos, cell.YPos] = cell;
        }

        public virtual int GetSize()
        {
            return _xDimension * _yDimension;
        }

        public virtual Vector2Int GetDimensions()
        {
            return new Vector2Int(_xDimension, _yDimension);
        }

        public virtual IGridCell GetLast()
        {
            return this[_xDimension - 1, _yDimension - 1];
        }

        public virtual IGridCell GetFirst()
        {
            return this[0, 0];
        }
      

        public IGridCell this[int x, int y]
        {
            get
            {
                if (x >= _xDimension || y >= _yDimension || x < 0 || y < 0)
                    throw new ArgumentOutOfRangeException($"x value: {x}, y value: {y}");

                return _items[x, y];
            }
            set
            {
                if (x >= _xDimension || y >= _yDimension || x < 0 || y < 0)
                    throw new ArgumentOutOfRangeException($"x value: {x}, y value: {y}");
                _items[x, y] = value;
            }
        }

        public bool TryGetNextCell(IGridCell cell,Direction direction, out IGridCell nextCell)
        {
            var neighbors = GetNeighbors(cell);
            foreach (var neighbor in neighbors)
            {
                if (neighbor.Direction == direction)
                {
                    nextCell = neighbor.Cell;
                    return true;
                }
            }

            nextCell = default;
            return false;
        }

        public List<Neighbor> GetNeighbors(IGridCell cell)
        {
            var neighbors = new List<Neighbor>();

            var x = cell.XPos;
            var y = cell.YPos;

            if (IsExists(x + 1, y)) neighbors.Add(new Neighbor(Direction.Right,this[x + 1, y]));
            if (IsExists(x - 1, y)) neighbors.Add(new Neighbor(Direction.Left,this[x - 1, y]));
            if (IsExists(x, y + 1)) neighbors.Add(new Neighbor(Direction.Forward,this[x, y + 1]));
            if (IsExists(x, y - 1)) neighbors.Add(new Neighbor(Direction.Back,this[x, y - 1]));

            return neighbors;
        }
        
        private bool IsExists(int xPos, int yPos)
        {
            return (xPos >= 0 && xPos < _xDimension && yPos >= 0 && yPos < _yDimension);
        }

        public IEnumerator<IGridCell> GetEnumerator()
        {
            for (var i = 0; i < _xDimension; i++)
            {
                for (var j = 0; j < _yDimension; j++)
                {
                    yield return _items[i, j];
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

    public enum Direction
    {
        Right,
        Left,
        Forward,
        Back
    }
}