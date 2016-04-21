using System.Collections.Generic;
using Mantle.Configuration.Interfaces;
using Mantle.Extensions;

namespace Mantle.Configuration.Configurers
{
    public class DefaultConfigurer<T> : BaseConfigurer<T>
    {
        private readonly IConfigurationProvider configurationProvider;

        public DefaultConfigurer(IConfigurationProvider configurationProvider)
        {
            configurationProvider.Require(nameof(configurationProvider));
            this.configurationProvider = configurationProvider;
        }

        public override IEnumerable<ConfigurationSetting> GetConfigurationSettings()
        {
            return configurationProvider.GetConfigurationSettings();
        }
    }
}