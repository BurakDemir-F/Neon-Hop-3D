using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General.GridSystem
{
    public class Grid<T> : IGrid<T> where T : IGridCell
    {
        protected T[,] _items;
        protected int _xDimension;
        protected int _yDimension;
        
        public Grid(IReadOnlyList<T> cells, int xDimension,int yDimension)
        {
            _xDimension = xDimension;
            _yDimension = yDimension;

            _items = new T[_xDimension, _yDimension];

            for (int i = 0; i < xDimension; i++)
            {
                for (int j = 0; j < yDimension; j++)
                {
                    var cell = cells[GetIndexYMovementFirst(_yDimension, i, j)];
                    _items[i, j] = cell;
                    cell.XPos = i;
                    cell.YPos = j;
                }
            }
        }

        private int GetIndexYMovementFirst(int yDimension, int xPos, int yPos)
        {
            return yDimension * xPos + yPos;
        }

        private int GetIndexXMovementFirst(int xDimension, int xPos, int yPos)
        {
            return xDimension * yPos + xPos;
        }
        
        public void SetCell(T cell)
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

        public virtual T GetLast()
        {
            return this[_xDimension - 1, _yDimension - 1];
        }

        public virtual T GetFirst()
        {
            return this[0, 0];
        }
      

        public T this[int x, int y]
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

        public bool TryGetNextCell(T cell,Direction direction, out T nextCell)
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

        public List<Neighbor<T>> GetNeighbors(T cell)
        {
            var neighbors = new List<Neighbor<T>>();

            var x = cell.XPos;
            var y = cell.YPos;

            if (IsExists(x + 1, y)) neighbors.Add(new Neighbor<T>(Direction.Right,this[x + 1, y]));
            if (IsExists(x - 1, y)) neighbors.Add(new Neighbor<T>(Direction.Left,this[x - 1, y]));
            if (IsExists(x, y + 1)) neighbors.Add(new Neighbor<T>(Direction.Forward,this[x, y + 1]));
            if (IsExists(x, y - 1)) neighbors.Add(new Neighbor<T>(Direction.Back,this[x, y - 1]));

            return neighbors;
        }
        
        private bool IsExists(int xPos, int yPos)
        {
            return (xPos >= 0 && xPos < _xDimension && yPos >= 0 && yPos < _yDimension);
        }

        public IEnumerator<T> GetEnumerator()
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