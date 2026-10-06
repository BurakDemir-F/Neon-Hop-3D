using Game.Pool.AddressablesLocal.Scripts.Runtime;
using Game.Sorcerum;
using UnityEngine;

namespace General
{
    public class GameContext : IContext
    {
        public GameContext(INextPositionProvider nextPositionProvider, IPoolCollection poolCollection)
        {
            NextPositionProvider = nextPositionProvider;
            PoolCollection = poolCollection;
            GameResolver = new GameResolver();
        }

        public INextPositionProvider NextPositionProvider { get; }
        public IPoolCollection PoolCollection { get; }
        public Vector3 BallStartPos => Vector3.zero;
        public IGameResolver GameResolver { get; }
    }

    public interface IGameResolver
    {
        void ResolveGame(BallBase ballBase, StackableItem item);
    }

    public class GameResolver : IGameResolver
    {
        public void ResolveGame(BallBase ballBase, StackableItem item)
        {
            var ballProvider = ballBase.AttributeCollection;
            var itemProvider = item.AttributeCollection;

            ballProvider.TryGetAttribute<ColorIdAttribute>(out var ballColor);
            itemProvider.TryGetAttribute<ColorIdAttribute>(out var itemColor);
            
            if (ballColor.IsMatching(itemColor.GetId()))
            {
                item.Crack();
                item.ReturnToPool();
            }

            ballBase.RemainingJump--;

            if (ballBase.RemainingJump <= 0)
            {
                ballBase.ReturnToPool();
            }
        }
    }
}