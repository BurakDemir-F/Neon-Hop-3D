namespace Game.Pool.AddressablesLocal.Scripts.Runtime
{
    public interface IPoolCollection
    {
        public IPoolObject Get(string key);
        public T Get<T>(string key) where T : IPoolObject;
        public void Return(IPoolObject poolObject);
        void ReturnAll();
        public IPool GetPool(string key);
        public void Release();
    }
}