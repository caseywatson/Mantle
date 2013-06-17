namespace Mantle
{
    public interface IDependencyResolver
    {
        T Get<T>();
    }
}