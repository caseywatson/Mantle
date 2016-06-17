using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Mantle.Configuration.Attributes;
using Mantle.Configuration.Interfaces;
using Mantle.Extensions;
using static System.String;

namespace Mantle.Configuration.Configurers
{
    public abstract class BaseConfigurer<T> : IConfigurer<T>
    {
        private readonly IPropertyConfigurer[] propertyConfigurers;

        protected readonly TypeMetadata TypeMetadata;

        protected BaseConfigurer()
            : this(null)
        {
        }

        protected BaseConfigurer(IPropertyConfigurer[] propertyConfigurers)
        {
            if (propertyConfigurers == null)
            {
                var localPropertyConfigurerTypes = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => typeof(IPropertyConfigurer).IsAssignableFrom(t) &&
                                (t.IsAbstract == false) &&
                                (t.IsInterface == false) &&
                                (t.GetConstructor(Type.EmptyTypes) != null)).ToList();

                propertyConfigurers = localPropertyConfigurerTypes // This relies on all local property configurers
                    .Select(Activator.CreateInstance) // having default constructors.
                    .Cast<IPropertyConfigurer>() // May need to rethink in the future.
                    .ToArray();
            }

            this.propertyConfigurers = propertyConfigurers;

            TypeMetadata = new TypeMetadata(typeof(T));
        }

        public abstract IEnumerable<ConfigurationSetting> GetConfigurationSettings();

        public virtual T Configure(T @object, string objectName = null)
        {
            var cfgObject = ToConfigurableObject(@object, objectName);
            var cfgSettings = GetConfigurationSettings();

            ApplyConfigurationSettings(cfgObject, cfgSettings);

            return cfgObject.Target;
        }

        protected virtual ConfigurableObject<T> ApplyConfigurationSettings(ConfigurableObject<T> cfgObject,
                                                                           IEnumerable<ConfigurationSetting>
                                                                               cfgSettings)
        {
            var cfgSettingsList = cfgSettings.ToList();

            foreach (var cfgProperty in cfgObject.Properties)
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

        private void ApplyConfigurationSetting(ConfigurableObject<T> cfgObject, ConfigurableProperty cfgProperty,
                                               ConfigurationSetting cfgSetting)
        {
            var propertyInfo = cfgProperty.PropertyMetadata.PropertyInfo;

            var propertyConfigurer = propertyConfigurers
                .FirstOrDefault(pc => pc.CanConfigureProperty(propertyInfo));

            if (propertyConfigurer == null)
            {
                throw new NotSupportedException(
                    $"Unable to apply configuration setting [{cfgSetting.Name}: {cfgSetting.Value}] to property " +
                    $"[{cfgObject.TypeMetadata.Type.Name}/{cfgProperty.PropertyMetadata.PropertyInfo.Name}]. " +
                    "The property is not supported.");
            }

            propertyConfigurer.Configure(cfgObject, cfgProperty, cfgSetting);
        }

        private IEnumerable<string> GetPrioritizedConventionalSettingNames(ConfigurableObject<T> cfgObject,
                                                                           ConfigurableProperty cfgProperty)
        {
            if (IsNullOrEmpty(cfgObject.Name) == false)
            {
                yield return $"{cfgObject.Name}." +
                             $"{cfgObject.TypeMetadata.Type.Name}." +
                             $"{cfgProperty.PropertyMetadata.PropertyInfo.Name}";

                yield return $"{cfgObject.Name}." +
                             $"{cfgProperty.PropertyMetadata.PropertyInfo.Name}";
            }

            yield return $"{cfgObject.TypeMetadata.Type.Name}." +
                         $"{cfgProperty.PropertyMetadata.PropertyInfo.Name}";

            yield return cfgProperty.PropertyMetadata.PropertyInfo.Name;
        }

        private ConfigurableObject<T> ToConfigurableObject(T @object, string objectName = null)
        {
            var cfgObject = new ConfigurableObject<T>();

            cfgObject.Name = objectName;
            cfgObject.Target = @object;
            cfgObject.TypeMetadata = new TypeMetadata(typeof(T));

            foreach (var propertyMetadata in cfgObject.TypeMetadata.Properties)
            {
                var cfgAttribute =
                    propertyMetadata.Attributes.OfType<ConfigurableAttribute>().SingleOrDefault();

                if (cfgAttribute != null)
                {
                    var cfgProperty = new ConfigurableProperty();

                    cfgProperty.IsRequired = cfgAttribute.IsRequired;
                    cfgProperty.PropertyMetadata = propertyMetadata;

                    if (IsNullOrEmpty(cfgAttribute.SettingName))
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