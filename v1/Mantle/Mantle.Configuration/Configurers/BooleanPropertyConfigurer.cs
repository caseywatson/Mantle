﻿using System;
using System.Configuration;
using System.Reflection;
using Mantle.Configuration.Interfaces;
using Mantle.Extensions;

namespace Mantle.Configuration.Configurers
{
    public class BooleanPropertyConfigurer : IPropertyConfigurer
    {
        public bool CanConfigureProperty(PropertyInfo propertyInfo)
        {
            propertyInfo.Require(nameof(propertyInfo));

            return ((propertyInfo.PropertyType == typeof(bool)) || (propertyInfo.PropertyType == typeof(bool?)));
        }

        public void Configure<T>(ConfigurableObject<T> cfgObject, ConfigurableProperty cfgProperty,
                                 ConfigurationSetting cfgSetting)
        {
            cfgObject.Require(nameof(cfgObject));
            cfgProperty.Require(nameof(cfgProperty));
            cfgSetting.Require(nameof(cfgSetting));

            var propertyType = cfgProperty.PropertyMetadata.PropertyInfo.PropertyType;
            var value = cfgSetting.Value.TryParseBoolean();

            if (propertyType == (typeof(bool)))
            {
                if (value == null)
                {
                    throw new ConfigurationErrorsException(
                        $"Unable to apply configuration setting [{cfgSetting.Name}: {cfgSetting.Value}] to property " +
                        $"[{cfgObject.TypeMetadata.Type.Name}/{cfgProperty.PropertyMetadata.PropertyInfo.Name}]. " +
                        $"[{cfgSetting.Value}] can not be converted to a [bool] value.");
                }

                cfgProperty.PropertyMetadata.PropertyInfo.SetValue(cfgObject.Target, value.Value);
            }
            else if (propertyType == (typeof(bool?)))
            {
                cfgProperty.PropertyMetadata.PropertyInfo.SetValue(cfgObject.Target, value);
            }
            else
            {
                throw new InvalidOperationException(
                    $"Unable to configure property [{cfgProperty.PropertyMetadata.PropertyInfo.Name}]. " +
                    "Property must be type [bool] or [bool?].");
            }
        }
    }
}