using Game.Pool.AddressablesLocal.Scripts.Runtime;
using Game.Sorcerum;
using UnityEngine;

namespace General
{
    public class GameContext : IContext
    {
        public GameContext(INextPositionProvider nextPositionProvider, IPoolCollection poolCollection, AvailableBallScreen availableBallScreen = null)
        {
            NextPositionProvider = nextPositionProvider;
            PoolCollection = poolCollection;
            AvailableBallScreen = availableBallScreen;
            GameResolver = new GameResolver();
        }

        public INextPositionProvider NextPositionProvider { get; }
        public IPoolCollection PoolCollection { get; }
        public AvailableBallScreen AvailableBallScreen { get; }
        public Vector3 BallStartPos => new Vector3(0f, 1f, -1.5f);
        public IGameResolver GameResolver { get; }
    }

    public interface IGameResolver
    {
        void ResolveGame(BallBase ballBase, IItemStack stack);
    }

    public class GameResolver : IGameResolver
    {
        public void ResolveGame(BallBase ballBase, IItemStack stack)
        {
            var ballProvider = ballBase.AttributeCollection;

            var hasItem = stack.HasItem();

            if (!hasItem)
            {
                DoBallOperations(ballBase);
                return;
            }
            
            stack.GetTop(out var item);
            
            var itemProvider = item.AttributeCollection;

            ballProvider.TryGetAttribute<ColorIdAttribute>(out var ballColor);
            itemProvider.TryGetAttribute<ColorIdAttribute>(out var itemColor);
            
            bool isMatching = false;
            
            if (ballColor != null && itemColor != null)
            {
                isMatching = ballColor.IsMatching(itemColor.GetId());
            }
            else
            {
                isMatching = true;
            }

            DoBallOperations(ballBase);
            
            if (isMatching)
            {
                item.CurrentStack?.RemoveFromStack();
                item.Crack();
                item.ReturnToPool();
            }
        }

        private void DoBallOperations(BallBase ballBase)
        {
            ballBase.RemainingJump--;
            
            if (ballBase.RemainingJump <= 0)
            {
                ballBase.ReturnToPool();
            }
        }
    }
}