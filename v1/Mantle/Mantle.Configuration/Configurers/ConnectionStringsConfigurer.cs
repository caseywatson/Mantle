using System.Collections.Generic;
using System.Configuration;
using Mantle.Interfaces;

namespace Mantle.Configuration.Configurers
{
    public class ConnectionStringsConfigurer<T> : BaseConfigurer<T>
    {
        public ConnectionStringsConfigurer()
        {
        }

        public ConnectionStringsConfigurer(ITypeMetadataCache typeMetadataCache)
            : base(typeMetadataCache)
        {
        }

        public override IEnumerable<ConfigurationSetting> GetConfigurationSettings(
            ConfigurationTarget<T> targetMetadata)
        {
            ConnectionStringSettingsCollection connectionStrings = ConfigurationManager.ConnectionStrings;

            foreach (ConfigurationTargetProperty targetPropertyMetadata in targetMetadata.Properties)
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