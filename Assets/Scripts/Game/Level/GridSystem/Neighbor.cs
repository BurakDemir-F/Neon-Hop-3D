namespace General.GridSystem
{
    public struct Neighbor<T> where T : IGridCell
    {
        public Direction Direction { get; private set; }
        public T Cell { get; private set; }

        public Neighbor(Direction direction, T cell)
        {
            Direction = direction;
            Cell = cell;
        }
    }
}