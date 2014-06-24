using Mantle.Interfaces;

namespace Mantle.Providers
{
    public class AssemblyQualifiedNameTypeTokenProvider : ITypeTokenProvider
    {
        public string GetTypeToken<T>()
        {
            return (typeof (T).AssemblyQualifiedName);
        }
    }
}