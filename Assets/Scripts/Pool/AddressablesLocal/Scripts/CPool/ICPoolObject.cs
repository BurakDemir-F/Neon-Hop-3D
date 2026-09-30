namespace Game.Pool.AddressablesLocal.Scripts.CPool
{
    public interface ICPoolObject
    {
        void OnReturnedToPool();
        void OnGet();
    }
}