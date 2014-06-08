using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Mantle
{
    public static class TypeExtensions
    {
        public static string GetMessagingTypeString(this Type type)
        {
            string typeName = type.FullName;
            string typeNamespace = String.Empty;

            DataContractAttribute dcAttribute =
                type.GetCustomAttributes(typeof (DataContractAttribute), false)
                    .OfType<DataContractAttribute>()
                    .FirstOrDefault();

            if (dcAttribute != null)
            {
                if (String.IsNullOrEmpty(dcAttribute.Name) == false)
                    typeName = dcAttribute.Name;

                if (String.IsNullOrEmpty(dcAttribute.Namespace) == false)
                    typeNamespace = dcAttribute.Namespace;
            }

            return HttpUtility.UrlEncode(String.Format("{0};{1}", typeName, typeNamespace));
        }
    }
}