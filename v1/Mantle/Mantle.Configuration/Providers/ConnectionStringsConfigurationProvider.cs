using System.Collections.Generic;
using System.Configuration;
using Mantle.Configuration.Interfaces;

namespace Mantle.Configuration.Providers
{
    public class ConnectionStringsConfigurationProvider : IConfigurationProvider
    {
        public IEnumerable<ConfigurationSetting> GetConfigurationSettings()
        {
            var connectionStrings = ConfigurationManager.ConnectionStrings;

            for (var i = 0; i < connectionStrings.Count; i++)
            {
                var connectionString = connectionStrings[i];
                yield return new ConfigurationSetting(connectionString.Name, connectionString.ConnectionString);
            }
        }
    }
}