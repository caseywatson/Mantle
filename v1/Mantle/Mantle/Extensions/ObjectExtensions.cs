using System;
using System.Collections.Generic;
using System.Reflection;

namespace Mantle.Extensions
{
    public static class ObjectExtensions
    {
        public static Dictionary<string, object> ToDictionary(this object source)
        {
            source.Require("source");

            var dictionary = new Dictionary<string, object>();
            Type sourceType = source.GetType();

            foreach (PropertyInfo propertyInfo in sourceType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                dictionary[propertyInfo.Name] = propertyInfo.GetValue(source);

            return dictionary;
        }
    }
}