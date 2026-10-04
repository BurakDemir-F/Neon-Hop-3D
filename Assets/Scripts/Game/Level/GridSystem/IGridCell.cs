using UnityEngine;

namespace General.GridSystem
{
    public interface IGridCell
    {
        public int XPos { get; set; }
        public int YPos { get; set; }

        public Vector2Int Position => new Vector2Int(XPos, YPos);

        public static bool operator <(IGridCell first, IGridCell second)
        {
            return second.XPos > first.XPos || (first.XPos == second.XPos && second.YPos > first.YPos);
        }

        public static bool operator >(IGridCell first, IGridCell second)
        {
            return first.XPos > second.XPos || (first.XPos == second.XPos && first.YPos > second.YPos);
        }
    }
}