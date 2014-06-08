using System;
using System.Collections.Generic;
using Mantle.Extensions;
using Mantle.Interfaces;

namespace Mantle.Configuration.Configurers
{
    public class AdHocConfigurer<T> : BaseConfigurer<T>
    {
        private readonly Dictionary<string, object> configurationSettings;

        public AdHocConfigurer(object configurationSettings)
        {
            if (configurationSettings == null)
                throw new ArgumentNullException("configurationSettings");

            this.configurationSettings = configurationSettings.ToDictionary();
        }

        public AdHocConfigurer(ITypeMetadataCache typeMetadataCache, object configurationSettings)
            : base(typeMetadataCache)
        {
            if (configurationSettings == null)
                throw new ArgumentNullException("configurationSettings");

            this.configurationSettings = configurationSettings.ToDictionary();
        }

        public override IEnumerable<ConfigurationSetting> GetConfigurationSettings(
            ConfigurationTarget<T> targetMetadata)
        {
            foreach (ConfigurationTargetProperty targetPropertyMetadata in targetMetadata.Properties)
            {
                if (configurationSettings.ContainsKey(targetPropertyMetadata.SettingName))
                {
                    yield return
                        new ConfigurationSetting
                        {
                            Name = targetPropertyMetadata.SettingName,
                            Value = configurationSettings[targetPropertyMetadata.SettingName].ToString()
                        };
                }
            }
        }
    }
}