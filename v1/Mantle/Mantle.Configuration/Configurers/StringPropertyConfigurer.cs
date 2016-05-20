using System;
using System.Reflection;
using Mantle.Configuration.Interfaces;
using Mantle.Extensions;

namespace Mantle.Configuration.Configurers
{
    public class StringPropertyConfigurer : IPropertyConfigurer
    {
        public bool CanConfigureProperty(PropertyInfo propertyInfo)
        {
            propertyInfo.Require(nameof(propertyInfo));

            return (propertyInfo.PropertyType == typeof(string));
        }

        public void Configure<T>(ConfigurableObject<T> cfgObject, ConfigurableProperty cfgProperty,
                                 ConfigurationSetting cfgSetting)
        {
            cfgObject.Require(nameof(cfgObject));
            cfgProperty.Require(nameof(cfgProperty));
            cfgSetting.Require(nameof(cfgSetting));

            var propertyInfo = cfgProperty.PropertyMetadata.PropertyInfo;

            if (propertyInfo.PropertyType == typeof(string))
            {
                propertyInfo.SetValue(cfgObject.Target, cfgSetting.Value);
            }
            else
            {
                throw new InvalidOperationException(
                    $"Unable to configure property [{propertyInfo.Name}]. " +
                    "Property must be type [string].");
            }
        }
    }
}