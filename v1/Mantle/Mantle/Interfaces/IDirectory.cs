namespace Mantle.Interfaces
{
    public interface IDirectory<T>
    {
        T this[string name] { get; }
    }
}