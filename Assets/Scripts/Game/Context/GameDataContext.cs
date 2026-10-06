using General;

namespace Game.Sorcerum
{
    public class GameDataContext : IContext
    {
        public int MaxPlayableBallCountAtTheSameTime { get; } = 5;
    }
}