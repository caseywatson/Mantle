using System.Collections.Generic;
using Mantle.Configuration.Interfaces;
using Mantle.Extensions;

namespace Mantle.Configuration.Providers
{
    public class CascadingConfigurationProvider : IConfigurationProvider
    {
        private readonly IConfigurationProvider[] childProviders;

        public CascadingConfigurationProvider(params IConfigurationProvider[] childProviders)
        {
            childProviders.Require("childProviders");
            this.childProviders = childProviders;
        }

        public IEnumerable<ConfigurationSetting> GetConfigurationSettings()
        {
            var configurationSettings = new Dictionary<string, ConfigurationSetting>();

            foreach (var childProvider in childProviders)
            {
                foreach (var configurationSetting in childProvider.GetConfigurationSettings())
                {
                    configurationSettings[configurationSetting.Name] = configurationSetting;
                }
            }

            return configurationSettings.Values;
        }
    }
}