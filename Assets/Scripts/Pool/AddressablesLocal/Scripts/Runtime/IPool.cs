namespace Game.Pool.AddressablesLocal.Scripts.Runtime
{
    public interface IPool
    {
        public IPoolObject Get();
        public T Get<T>() where T : IPoolObject;
        public void Return(IPoolObject poolObject);

        void ReturnAll();
        void Release();
    }
}