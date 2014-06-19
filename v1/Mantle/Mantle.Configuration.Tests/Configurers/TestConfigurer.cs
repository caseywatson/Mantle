using System.Collections.Generic;
using Mantle.Configuration.Configurers;

namespace Mantle.Configuration.Tests.Configurers
{
    public class TestConfigurer<T> : BaseConfigurer<T>
    {
        private readonly IEnumerable<ConfigurationSetting> configurationSettings;

        public TestConfigurer(IEnumerable<ConfigurationSetting> configurationSettings)
        {
            this.configurationSettings = configurationSettings;
        }

        public override IEnumerable<ConfigurationSetting> GetConfigurationSettings(ConfigurationTarget<T> target)
        {
            return configurationSettings;
        }
    }
}