using System.Collections.Generic;
using UnityEngine;

namespace General.GridSystem
{
    public abstract class GridPositionProvider
    {
        protected Transform _originTransform;
        protected Vector3 _originPosition;
        protected float _cellSize;
        protected float _padding;

        protected GridPositionProvider(Transform originTransform, float cellSize, float padding)
        {
            _originTransform = originTransform;
            _originPosition = _originTransform.position;
            _cellSize = cellSize;
            _padding = padding;
        }

        public abstract Vector3 GetWorldPosition(Vector2Int positionOnGrid);

        public Vector3 GetWorldPosition(int index, int xDimension)
        {
            var y = index / xDimension;
            var x = index % xDimension;

            return GetWorldPosition(new Vector2Int(x, y));
        }
        
        protected float GetCellSizeOffset()
        {
            return _cellSize * .5f;
        }
    }
}