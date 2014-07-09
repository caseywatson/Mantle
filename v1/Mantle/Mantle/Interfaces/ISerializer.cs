namespace Mantle.Interfaces
{
    public interface ISerializer<T>
    {
        T Deserialize(string source);
        string Serialize(T source);
    }
}