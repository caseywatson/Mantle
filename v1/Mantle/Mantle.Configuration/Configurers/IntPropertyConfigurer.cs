using Mantle.Configuration.Interfaces;
using Mantle.Extensions;
using System;
using System.Configuration;
using System.Reflection;

namespace Mantle.Configuration.Configurers
{
    public class IntPropertyConfigurer : IPropertyConfigurer
    {
        public bool CanConfigureProperty(PropertyInfo propertyInfo)
        {
            propertyInfo.Require(nameof(propertyInfo));

            return ((propertyInfo.PropertyType == typeof(int)) || (propertyInfo.PropertyType == typeof(int?)));
        }

        public void Configure<T>(ConfigurableObject<T> cfgObject, ConfigurableProperty cfgProperty, ConfigurationSetting cfgSetting)
        {
            cfgObject.Require(nameof(cfgObject));
            cfgProperty.Require(nameof(cfgProperty));
            cfgSetting.Require(nameof(cfgSetting));

            Type propertyType = cfgProperty.PropertyMetadata.PropertyInfo.PropertyType;
            int? value = cfgSetting.Value.TryParseInt();

            if (propertyType == (typeof(int)))
            {
                if (value == null)
                {
                    throw new ConfigurationErrorsException(
                        $"Unable to apply configuration setting [{cfgSetting.Name}: {cfgSetting.Value}] to property " +
                        $"[{cfgObject.TypeMetadata.Type.Name}/{cfgProperty.PropertyMetadata.PropertyInfo.Name}]. " +
                        $"[{cfgSetting.Value}] can not be converted to a [int] value.");
                }

                cfgProperty.PropertyMetadata.PropertyInfo.SetValue(cfgObject.Target, value.Value);
            }
            else if (propertyType == (typeof(int?)))
            {
                cfgProperty.PropertyMetadata.PropertyInfo.SetValue(cfgObject.Target, value);
            }
            else
            {
                throw new InvalidOperationException(
                    $"Unable to configure property [{cfgProperty.PropertyMetadata.PropertyInfo.Name}]. " +
                    "Property must be type [int] or [int?].");
            }
        }
    }
}
