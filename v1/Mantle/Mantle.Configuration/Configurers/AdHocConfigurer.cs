using System;
using System.Collections.Generic;
using Mantle.Extensions;

namespace Mantle.Configuration.Configurers
{
    public class AdHocConfigurer<T> : BaseConfigurer<T>
    {
        private readonly Dictionary<string, object> configurationDictionary;

        public AdHocConfigurer(object configurationObject)
        {
            configurationObject.Require("configurationObject");
            configurationDictionary = configurationObject.ToDictionary();
        }

        public override IEnumerable<ConfigurationSetting> GetConfigurationSettings(
            ConfigurationTarget<T> targetMetadata)
        {
            foreach (ConfigurationTargetProperty targetPropertyMetadata in targetMetadata.Properties)
            {
                if (configurationDictionary.ContainsKey(targetPropertyMetadata.SettingName))
                {
                    yield return
                        new ConfigurationSetting
                        {
                            Name = targetPropertyMetadata.SettingName,
                            Value = configurationDictionary[targetPropertyMetadata.SettingName].ToString()
                        };
                }
            }
        }
    }
}