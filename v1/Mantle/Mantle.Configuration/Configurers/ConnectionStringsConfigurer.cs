using System.Collections.Generic;
using System.Configuration;

namespace Mantle.Configuration.Configurers
{
    public class ConnectionStringsConfigurer<T> : BaseConfigurer<T>
    {
        public override IEnumerable<ConfigurationSetting> GetConfigurationSettings(
            ConfigurableObject<T> targetMetadata)
        {
            ConnectionStringSettingsCollection connectionStrings = ConfigurationManager.ConnectionStrings;

            foreach (ConfigurableProperty targetPropertyMetadata in targetMetadata.Properties)
            {
                if (connectionStrings[targetPropertyMetadata.SettingName] != null)
                {
                    yield return
                        new ConfigurationSetting
                        {
                            Name = targetPropertyMetadata.SettingName,
                            Value = connectionStrings[targetPropertyMetadata.SettingName].ConnectionString
                        };
                }
            }
        }
    }
}