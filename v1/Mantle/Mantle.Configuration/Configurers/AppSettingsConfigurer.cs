using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace Mantle.Configuration.Configurers
{
    public class AppSettingsConfigurer<T> : BaseConfigurer<T>
    {
        public override IEnumerable<ConfigurationSetting> GetConfigurationSettings(
            ConfigurableObject<T> targetMetadata)
        {
            NameValueCollection appSettings = ConfigurationManager.AppSettings;

            foreach (ConfigurableProperty targetPropertyMetadata in targetMetadata.Properties)
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