using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace Mantle.Configuration.Configurers
{
    public class AppSettingsConfigurer<T> : BaseConfigurer<T>
    {
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