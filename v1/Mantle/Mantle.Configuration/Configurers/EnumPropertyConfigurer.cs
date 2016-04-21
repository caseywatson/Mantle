using System;
using System.Configuration;
using System.Reflection;
using Mantle.Configuration.Interfaces;
using Mantle.Extensions;

namespace Mantle.Configuration.Configurers
{
    public class EnumPropertyConfigurer : IPropertyConfigurer
    {
        public bool CanConfigureProperty(PropertyInfo propertyInfo)
        {
            propertyInfo.Require(nameof(propertyInfo));

            var nullableType = Nullable.GetUnderlyingType(propertyInfo.PropertyType);

            return ((propertyInfo.PropertyType.IsEnum) || (nullableType?.IsEnum == true));
        }

        public void Configure<T>(ConfigurableObject<T> cfgObject, ConfigurableProperty cfgProperty,
            ConfigurationSetting cfgSetting)
        {
            cfgObject.Require(nameof(cfgObject));
            cfgProperty.Require(nameof(cfgProperty));
            cfgSetting.Require(nameof(cfgSetting));

            var propertyInfo = cfgProperty.PropertyMetadata.PropertyInfo;
            var propertyType = propertyInfo.PropertyType;

            if (propertyType.IsEnum)
            {
                var value = TryParseEnum(propertyType, cfgSetting.Value);

                if (value == null)
                {
                    throw new ConfigurationErrorsException(
                        $"Unable to apply configuration setting [{cfgSetting.Name}: {cfgSetting.Value}] to property " +
                        $"[{cfgObject.TypeMetadata.Type.Name}/{propertyInfo.Name}]. " +
                        $"[{cfgSetting.Value}] can not be converted to a [{propertyType.Name}] value.");
                }

                propertyInfo.SetValue(cfgObject.Target, value);
            }
            else if (Nullable.GetUnderlyingType(propertyType)?.IsEnum == true)
            {
                var enumPropertyType = Nullable.GetUnderlyingType(propertyType);
                var value = TryParseEnum(enumPropertyType, cfgSetting.Value);

                propertyInfo.SetValue(cfgObject.Target, value);
            }
            else
            {
                throw new InvalidOperationException(
                    $"Unable to configure property [{cfgProperty.PropertyMetadata.PropertyInfo.Name}]. " +
                    "Property must be an [enum] or [enum?] type.");
            }
        }

        private object TryParseEnum(Type type, string value)
        {
            if (Enum.IsDefined(type, value))
                return Enum.Parse(type, value);

            return null;
        }
    }
}