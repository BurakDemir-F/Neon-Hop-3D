namespace General
{
    public interface IContextProvider
    {
        T GetContext<T>() where T : IContext;
    }

    public interface IContext
    {
        
    }

    public class GameContext : IContext
    {
        
    }
}