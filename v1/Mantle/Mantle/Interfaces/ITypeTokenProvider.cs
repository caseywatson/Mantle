namespace Mantle.Interfaces
{
    public interface ITypeTokenProvider
    {
        string GetTypeToken<T>();
    }
}