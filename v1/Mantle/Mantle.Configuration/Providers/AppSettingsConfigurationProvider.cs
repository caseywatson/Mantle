using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Mantle.Configuration.Interfaces;

namespace Mantle.Configuration.Providers
{
    public class AppSettingsConfigurationProvider : IConfigurationProvider
    {
        public IEnumerable<ConfigurationSetting> GetConfigurationSettings()
        {
            var appSettings = ConfigurationManager.AppSettings;
            return appSettings.Keys.OfType<string>().Select(k => new ConfigurationSetting(k, appSettings[k]));
        }
    }
}