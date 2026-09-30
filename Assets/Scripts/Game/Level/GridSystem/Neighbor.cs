namespace General.GridSystem
{
    public struct Neighbor
    {
        public Direction Direction { get; private set; }
        public IGridCell Cell { get; private set; }

        public Neighbor(Direction direction, IGridCell cell)
        {
            Direction = direction;
            Cell = cell;
        }
    }
}