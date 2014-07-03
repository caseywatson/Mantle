namespace Mantle.Interfaces
{
    public interface ISerializer<T>
    {
        string Serialize(T source);
        T Deserialize(string source);
    }
}