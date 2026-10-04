using Game.Sorcerum;

namespace General
{
    public class ContextProvider : IContextProvider
    {
        private TypeObjectCollection<IContext> _contextCollection = new();
        public ContextProvider(INextPositionProvider nextPositionProvider)
        {
            _contextCollection.UpdateObject(new GameContext(nextPositionProvider));
        }
        public bool TryGetContext<T>(out T context) where T : IContext
        {
            return _contextCollection.TryGetObject(out context);
        }
    }

    public interface IContextProvider
    {
        bool TryGetContext<T>(out T context) where T : IContext;
    }

    public interface IContext
    {
        
    }

    public class GameContext : IContext
    {
        public GameContext(INextPositionProvider nextPositionProvider)
        {
            NextPositionProvider = nextPositionProvider;
        }

        public INextPositionProvider NextPositionProvider { get; }
    }
}