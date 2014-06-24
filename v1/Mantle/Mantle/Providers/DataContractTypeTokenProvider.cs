using System;
using System.Linq;
using System.Runtime.Serialization;
using Mantle.Interfaces;

namespace Mantle.Providers
{
    public class DataContractTypeTokenProvider : ITypeTokenProvider
    {
        public string GetTypeToken<T>()
        {
            DataContractAttribute dataContractAttribute =
                (typeof (T)).GetCustomAttributes(typeof (DataContractAttribute), false)
                    .OfType<DataContractAttribute>()
                    .FirstOrDefault();

            if (dataContractAttribute == null)
                return null;

            string dataContractName = (dataContractAttribute.Name ?? (typeof (T).Name));

            if (String.IsNullOrEmpty(dataContractAttribute.Namespace))
                return dataContractName;

            return String.Format("{0}|{1}", dataContractAttribute.Namespace, dataContractName);
        }
    }
}