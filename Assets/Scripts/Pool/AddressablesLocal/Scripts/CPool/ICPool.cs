namespace Game.Pool.AddressablesLocal.Scripts.CPool
{
    public interface ICPool<T> where T: ICPoolObject,new()
    {
        T Get();
        void Return(T poolObject);
    }
}