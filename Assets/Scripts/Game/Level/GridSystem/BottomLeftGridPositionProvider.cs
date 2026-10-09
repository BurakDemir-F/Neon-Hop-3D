using UnityEngine;

namespace General.GridSystem
{
    public class BottomLeftGridPositionProvider : GridPositionProvider
    {
        public BottomLeftGridPositionProvider( Transform originTransform, float cellSize,
            float padding) : base(originTransform, cellSize, padding)
        {
            
        }

        public override Vector3 GetWorldPosition(Vector2Int positionOnGrid)
        {
            var x = positionOnGrid.x;
            var y = positionOnGrid.y;
            var cellSizeOffset = GetCellSizeOffset();
            var origin = _originTransform != null ? _originTransform.position : _originPosition;
            
            return new Vector3(origin.x + (x * (_cellSize + _padding)) + cellSizeOffset,
                origin.y, origin.z + (y * (_cellSize + _padding)) + cellSizeOffset);
        }
    }
}