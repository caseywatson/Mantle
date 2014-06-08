using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using Mantle.Interfaces;

namespace Mantle.Configuration.Configurers
{
    public class AppSettingsConfigurer<T> : BaseConfigurer<T>
    {
        public AppSettingsConfigurer()
        {
        }

        public AppSettingsConfigurer(ITypeMetadataCache typeMetadataCache)
            : base(typeMetadataCache)
        {
        }

        public override IEnumerable<ConfigurationSetting> GetConfigurationSettings(
            ConfigurationTarget<T> targetMetadata)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;

            foreach (ConfigurationTargetProperty targetPropertyMetadata in targetMetadata.Properties)
            {
                if (appSettings[targetPropertyMetadata.SettingName] != null)
                {
                    yield return
                        new ConfigurationSetting
                        {
                            Name = targetPropertyMetadata.SettingName,
                            Value = appSettings[targetPropertyMetadata.SettingName]
                        };
                }
            }
        }
    }
}