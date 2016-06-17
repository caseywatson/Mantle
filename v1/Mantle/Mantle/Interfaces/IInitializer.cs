namespace Mantle.Interfaces
{
    public interface IInitializer<T>
    {
        void Initialize(T toInitialize);
    }
}