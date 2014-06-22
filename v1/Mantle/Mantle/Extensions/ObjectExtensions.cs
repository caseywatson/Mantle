using System;
using System.Collections.Generic;
using System.Reflection;

namespace Mantle.Extensions
{
    public static class ObjectExtensions
    {
        public static Dictionary<string, object> ToDictionary(this object source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var dictionary = new Dictionary<string, object>();
            Type sourceType = source.GetType();

            foreach (PropertyInfo propertyInfo in sourceType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                dictionary[propertyInfo.Name] = propertyInfo.GetValue(source);

            return dictionary;
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
    }
}