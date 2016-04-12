using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Mantle.Configuration.Attributes;
using Mantle.Configuration.Interfaces;
using Mantle.Extensions;
using static System.String;

namespace Mantle.Configuration.Configurers
{
    public abstract class BaseConfigurer<T> : IConfigurer<T>
    {
        protected readonly TypeMetadata TypeMetadata;

        protected BaseConfigurer()
        {
            TypeMetadata = new TypeMetadata(typeof (T));
        }

        public abstract IEnumerable<ConfigurationSetting> GetConfigurationSettings();

        public virtual T Configure(T @object, string objectName = null)
        {
            ConfigurableObject<T> cfgObject = ToConfigurableObject(@object, objectName);
            IEnumerable<ConfigurationSetting> cfgSettings = GetConfigurationSettings();

            ApplyConfigurationSettings(cfgObject, cfgSettings);

            return cfgObject.Target;
        }

        protected virtual ConfigurableObject<T> ApplyConfigurationSettings(ConfigurableObject<T> cfgObject,
                                                                           IEnumerable<ConfigurationSetting>
                                                                               cfgSettings)
        {
            List<ConfigurationSetting> cfgSettingsList = cfgSettings.ToList();

            foreach (ConfigurableProperty cfgProperty in cfgObject.Properties)
            {
                var cfgSettingDictionary = cfgSettingsList
                    .Where(cs => cfgProperty.PrioritizedSettingNames.Contains(cs.Name))
                    .ToDictionary(cs => cs.Name);

                if (cfgSettingDictionary.Any())
                {
                    var settingName = cfgProperty.PrioritizedSettingNames.First(cfgSettingDictionary.ContainsKey);

                    ApplyConfigurationSetting(cfgObject, cfgProperty, cfgSettingDictionary[settingName]);
                }
                else if (cfgProperty.IsRequired)
                {
                    throw new ConfigurationErrorsException(
                        "Unable to configure the required property " +
                        $"[{cfgObject.TypeMetadata.Type.Name}/{cfgProperty.PropertyMetadata.PropertyInfo.Name}]. " +
                        "None of the following configuration setting(s) were found: " +
                        $"[{Join(", ", cfgProperty.PrioritizedSettingNames)}].");
                }
            }

            return cfgObject;
        }

        private void ApplyBooleanConfigurationSetting(ConfigurableObject<T> cfgObject, ConfigurableProperty cfgProperty,
                                                      ConfigurationSetting cfgSetting)
        {
            Type propertyType = cfgProperty.PropertyMetadata.PropertyInfo.PropertyType;
            bool? value = cfgSetting.Value.TryParseBoolean();

            if (propertyType == (typeof (bool)))
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
            else if (propertyType == (typeof (bool?)))
            {
                cfgProperty.PropertyMetadata.PropertyInfo.SetValue(cfgObject.Target, value);
            }
        }

        private void ApplyConfigurationSetting(ConfigurableObject<T> cfgObject, ConfigurableProperty cfgProperty,
                                               ConfigurationSetting cfgSetting)
        {
            Type propertyType = cfgProperty.PropertyMetadata.PropertyInfo.PropertyType;

            if ((propertyType == typeof (bool)) || (propertyType == typeof (bool?)))
            {
                ApplyBooleanConfigurationSetting(cfgObject, cfgProperty, cfgSetting);
            }
            else if ((propertyType == typeof (DateTime)) || (propertyType == typeof (DateTime?)))
            {
                ApplyDateTimeConfigurationSetting(cfgObject, cfgProperty, cfgSetting);
            }
            else if ((propertyType == typeof (double)) || (propertyType == typeof (double?)))
            {
                ApplyDoubleConfigurationSetting(cfgObject, cfgProperty, cfgSetting);
            }
            else if ((propertyType == typeof (Guid)) || (propertyType == typeof (Guid?)))
            {
                ApplyGuidConfigurationSetting(cfgObject, cfgProperty, cfgSetting);
            }
            else if ((propertyType == typeof (int)) || (propertyType == typeof (int?)))
            {
                ApplyIntConfigurationSetting(cfgObject, cfgProperty, cfgSetting);
            }
            else if ((propertyType == typeof (long)) || (propertyType == typeof (long?)))
            {
                ApplyLongConfigurationSettings(cfgObject, cfgProperty, cfgSetting);
            }
            else if ((propertyType == typeof (string)))
            {
                cfgProperty.PropertyMetadata.PropertyInfo.SetValue(cfgObject.Target, cfgSetting.Value);
            }
            else if ((propertyType.IsEnum))
            {

            }
            else
            {
                throw new ConfigurationErrorsException(
                    $"Unable to apply configuration setting [{cfgSetting.Name}: {cfgSetting.Value}] to property " +
                    $"[{cfgObject.TypeMetadata.Type.Name}/{cfgProperty.PropertyMetadata.PropertyInfo.Name}]. " +
                    "The target must be of type [bool], [bool?], [DateTime], [DateTime?], [double], [double?], [Guid], [Guid?], " +
                    "[int], [int?], [long], [long?], [string] or an enumeration type.");
            }
        }

        private void ApplyDateTimeConfigurationSetting(ConfigurableObject<T> cfgObject, ConfigurableProperty cfgProperty,
                                                       ConfigurationSetting cfgSetting)
        {
            Type propertyType = cfgProperty.PropertyMetadata.PropertyInfo.PropertyType;
            DateTime? value = cfgSetting.Value.TryParseDateTime();

            if (propertyType == (typeof (DateTime)))
            {
                if (value == null)
                {
                    throw new ConfigurationErrorsException(
                        $"Unable to apply configuration setting [{cfgSetting.Name}: {cfgSetting.Value}] to property " +
                        $"[{cfgObject.TypeMetadata.Type.Name}/{cfgProperty.PropertyMetadata.PropertyInfo.Name}]. " +
                        $"[{cfgSetting.Value}] can not be converted to a [DateTime] value.");
                }

                cfgProperty.PropertyMetadata.PropertyInfo.SetValue(cfgObject.Target, value.Value);
            }
            else if (propertyType == (typeof (DateTime?)))
            {
                cfgProperty.PropertyMetadata.PropertyInfo.SetValue(cfgObject.Target, value);
            }
        }

        private void ApplyDoubleConfigurationSetting(ConfigurableObject<T> cfgObject, ConfigurableProperty cfgProperty,
                                                     ConfigurationSetting cfgSetting)
        {
            Type propertyType = cfgProperty.PropertyMetadata.PropertyInfo.PropertyType;
            double? value = cfgSetting.Value.TryParseDouble();

            if (propertyType == (typeof (double)))
            {
                if (value == null)
                {
                    throw new ConfigurationErrorsException(
                        $"Unable to apply configuration setting [{cfgSetting.Name}: {cfgSetting.Value}] to property " +
                        $"[{cfgObject.TypeMetadata.Type.Name}/{cfgProperty.PropertyMetadata.PropertyInfo.Name}]. " +
                        $"[{cfgSetting.Value}] can not be converted to a [double] value.");
                }

                cfgProperty.PropertyMetadata.PropertyInfo.SetValue(cfgObject.Target, value.Value);
            }
            else if (propertyType == (typeof (double?)))
            {
                cfgProperty.PropertyMetadata.PropertyInfo.SetValue(cfgObject.Target, value);
            }
        }

        private void ApplyGuidConfigurationSetting(ConfigurableObject<T> cfgObject, ConfigurableProperty cfgProperty,
                                                   ConfigurationSetting cfgSetting)
        {
            Type propertyType = cfgProperty.PropertyMetadata.PropertyInfo.PropertyType;
            Guid? value = cfgSetting.Value.TryParseGuid();

            if (propertyType == (typeof (Guid)))
            {
                if (value == null)
                {
                    throw new ConfigurationErrorsException(
                        $"Unable to apply configuration setting [{cfgSetting.Name}: {cfgSetting.Value}] to property " +
                        $"[{cfgObject.TypeMetadata.Type.Name}/{cfgProperty.PropertyMetadata.PropertyInfo.Name}]. " +
                        $"[{cfgSetting.Value}] can not be converted to a [Guid] value.");
                }

                cfgProperty.PropertyMetadata.PropertyInfo.SetValue(cfgObject.Target, value.Value);
            }
            else if (propertyType == (typeof (Guid?)))
            {
                cfgProperty.PropertyMetadata.PropertyInfo.SetValue(cfgObject.Target, value);
            }
        }

        private void ApplyIntConfigurationSetting(ConfigurableObject<T> cfgObject, ConfigurableProperty cfgProperty,
                                                  ConfigurationSetting cfgSetting)
        {
            Type propertyType = cfgProperty.PropertyMetadata.PropertyInfo.PropertyType;
            int? value = cfgSetting.Value.TryParseInt();

            if (propertyType == (typeof (int)))
            {
                if (value == null)
                {
                    throw new ConfigurationErrorsException(
                        $"Unable to apply configuration setting [{cfgSetting.Name}: {cfgSetting.Value}] to property " +
                        $"[{cfgObject.TypeMetadata.Type.Name}/{cfgProperty.PropertyMetadata.PropertyInfo.Name}]. " +
                        $"[{cfgSetting.Value}] can not be converted to an [int] value.");
                }

                cfgProperty.PropertyMetadata.PropertyInfo.SetValue(cfgObject.Target, value.Value);
            }
            else if (propertyType == (typeof (int?)))
            {
                cfgProperty.PropertyMetadata.PropertyInfo.SetValue(cfgObject.Target, value);
            }
        }

        private void ApplyLongConfigurationSettings(ConfigurableObject<T> cfgObject, ConfigurableProperty cfgProperty,
                                                    ConfigurationSetting cfgSetting)
        {
            Type propertyType = cfgProperty.PropertyMetadata.PropertyInfo.PropertyType;
            long? value = cfgSetting.Value.TryParseLong();

            if (propertyType == (typeof (long)))
            {
                if (value == null)
                {
                    throw new ConfigurationErrorsException(
                        $"Unable to apply configuration setting [{cfgSetting.Name}: {cfgSetting.Value}] to property " +
                        $"[{cfgObject.TypeMetadata.Type.Name}/{cfgProperty.PropertyMetadata.PropertyInfo.Name}]. " +
                        $"[{cfgSetting.Value}] can not be converted to a [long] value.");
                }

                cfgProperty.PropertyMetadata.PropertyInfo.SetValue(cfgObject.Target, value.Value);
            }
            else if (propertyType == (typeof (long?)))
            {
                cfgProperty.PropertyMetadata.PropertyInfo.SetValue(cfgObject.Target, value);
            }
        }

        private void ApplyEnumerationConfigurationSetting(ConfigurableObject<T> cfgObject, ConfigurableProperty cfgProperty,
                                                          ConfigurationSetting cfgSetting)
        {
            Type propertyType = cfgProperty.PropertyMetadata.PropertyInfo.PropertyType;

            if (Enum.IsDefined(propertyType, cfgSetting.Value) == false)
            {
                throw new ConfigurationErrorsException(
                    $"Unable to apply configuration setting [{cfgSetting.Name}: {cfgSetting.Value}] to property " +
                    $"[{cfgObject.TypeMetadata.Type.Name}/{cfgProperty.PropertyMetadata.PropertyInfo.Name}]. " +
                    $"[{cfgSetting.Value}] can not be converted to a [{propertyType.Name}] value.");
            }

            cfgProperty.PropertyMetadata.PropertyInfo.SetValue(cfgObject.Target, Enum.Parse(propertyType, cfgSetting.Value));
        }

        private IEnumerable<string> GetPrioritizedConventionalSettingNames(ConfigurableObject<T> cfgObject,
                                                                           ConfigurableProperty cfgProperty)
        {
            if (IsNullOrEmpty(cfgObject.Name) == false)
            {
                yield return $"{cfgObject.Name}.{cfgObject.TypeMetadata.Type.Name}.{cfgProperty.PropertyMetadata.PropertyInfo.Name}";
                yield return $"{cfgObject.Name}.{cfgProperty.PropertyMetadata.PropertyInfo.Name}";
            }

            yield return $"{cfgObject.TypeMetadata.Type.Name}.{cfgProperty.PropertyMetadata.PropertyInfo.Name}";
            yield return cfgProperty.PropertyMetadata.PropertyInfo.Name;
        }

        private ConfigurableObject<T> ToConfigurableObject(T @object, string objectName = null)
        {
            var cfgObject = new ConfigurableObject<T>();

            cfgObject.Name = objectName;
            cfgObject.Target = @object;
            cfgObject.TypeMetadata = new TypeMetadata(typeof (T));

            foreach (PropertyMetadata propertyMetadata in cfgObject.TypeMetadata.Properties)
            {
                ConfigurableAttribute cfgAttribute =
                    propertyMetadata.Attributes.OfType<ConfigurableAttribute>().SingleOrDefault();

                if (cfgAttribute != null)
                {
                    var cfgProperty = new ConfigurableProperty();

                    cfgProperty.IsRequired = cfgAttribute.IsRequired;
                    cfgProperty.PropertyMetadata = propertyMetadata;

                    if (String.IsNullOrEmpty(cfgAttribute.SettingName))
                    {
                        cfgProperty.PrioritizedSettingNames =
                            GetPrioritizedConventionalSettingNames(cfgObject, cfgProperty).ToArray();
                    }
                    else
                    {
                        cfgProperty.PrioritizedSettingNames = new[]
                        {cfgAttribute.SettingName.Merge(new {Name = objectName})};
                    }

                    cfgObject.Properties.Add(cfgProperty);
                }
            }

            return cfgObject;
        }
    }
}