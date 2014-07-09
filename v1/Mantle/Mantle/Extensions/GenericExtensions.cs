using System;
using Newtonsoft.Json;

namespace Mantle.Extensions
{
    public static class GenericExtensions
    {
        public static T FromJson<T>(this string objectString)
        {
            objectString.Require("objectString");
            return JsonConvert.DeserializeObject<T>(objectString);
        }

        public static void Require<T>(this T parameter, string parameterName, string errorMessage = null)
            where T : class
        {
            if (String.IsNullOrEmpty(parameterName))
                throw new ArgumentException("[parameterName] is required.", "parameterName");

            if (parameter == null)
            {
                if (errorMessage == null)
                    throw new ArgumentNullException(parameterName);

                throw new ArgumentNullException(parameterName, errorMessage);
            }
        }

        public static string ToJson<T>(this T @object)
        {
            return JsonConvert.SerializeObject(@object);
        }
    }
}