using System.Collections.Generic;
using System.Configuration;
using Mantle.Configuration.Interfaces;

namespace Mantle.Configuration.Providers
{
    public class ConnectionStringsConfigurationProvider : IConfigurationProvider
    {
        public IEnumerable<ConfigurationSetting> GetConfigurationSettings()
        {
            ConnectionStringSettingsCollection connectionStrings = ConfigurationManager.ConnectionStrings;

            for (int i = 0; i < connectionStrings.Count; i++)
            {
                ConnectionStringSettings connectionString = connectionStrings[i];
                yield return new ConfigurationSetting(connectionString.Name, connectionString.ConnectionString);
            }
        }
    }
}