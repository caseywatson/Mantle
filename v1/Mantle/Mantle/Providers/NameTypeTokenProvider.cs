using Mantle.Interfaces;

namespace Mantle.Providers
{
    public class NameTypeTokenProvider : ITypeTokenProvider
    {
        public string GetTypeToken<T>()
        {
            return (typeof (T).Name);
        }
    }
}