using System.Linq;
using System.Runtime.Serialization;
using Mantle.Interfaces;

namespace Mantle.Providers
{
    public class DataContractTypeTokenProvider : ITypeTokenProvider
    {
        public string GetTypeToken<T>()
        {
            var dataContractAttribute =
                (typeof(T)).GetCustomAttributes(typeof(DataContractAttribute), false)
                    .OfType<DataContractAttribute>()
                    .FirstOrDefault();

            if (dataContractAttribute == null)
                return null;

            var dataContractName = (dataContractAttribute.Name ?? (typeof(T).Name));

            if (string.IsNullOrEmpty(dataContractAttribute.Namespace))
                return dataContractName;

            return string.Format("{0}|{1}", dataContractAttribute.Namespace, dataContractName);
        }
    }
}