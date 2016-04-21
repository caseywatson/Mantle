using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Mantle.Configuration.Configurers
{
    public class AppSettingsConfigurer<T> : BaseConfigurer<T>
    {
        public override IEnumerable<ConfigurationSetting> GetConfigurationSettings()
        {
            var appSettings = ConfigurationManager.AppSettings;
            return appSettings.Keys.OfType<string>().Select(k => new ConfigurationSetting(k, appSettings[k]));
        }
    }
}