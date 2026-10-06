using System;
using Game.Sorcerum;

namespace General
{
    public class ContextProvider : IContextProvider
    {
        private TypeObjectCollection<IContext> _contextCollection = new();
        public ContextProvider()
        {
            _contextCollection.UpdateObject<GameEventBus>(new GameEventBus());
        }

        public void UpdateContext<T>(T context) where T : IContext
        {
            _contextCollection.UpdateObject<T>(context);
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
}