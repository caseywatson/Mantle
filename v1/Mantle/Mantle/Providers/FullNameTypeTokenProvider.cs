using Mantle.Interfaces;

namespace Mantle.Providers
{
    public class FullNameTypeTokenProvider : ITypeTokenProvider
    {
        public string GetTypeToken<T>()
        {
            return (typeof(T).FullName);
        }
    }
}