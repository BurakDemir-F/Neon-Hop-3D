using Game.Sorcerum;

namespace General
{
    public class GameEventBus : EventBus, IContext
    {
        
    }

    public readonly struct BallSelectedEvent
    {
        public IAttributeProvider AttributeProvider { get; }

        public BallSelectedEvent(IAttributeProvider attributeProvider)
        {
            AttributeProvider = attributeProvider;
        }
    }
}