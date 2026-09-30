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
            
            return new Vector3(_originPosition.x + (x * (_cellSize + _padding)) + cellSizeOffset,
                _originPosition.y , _originPosition.z + (y * (_cellSize + _padding)) + cellSizeOffset);
        }
    }
}