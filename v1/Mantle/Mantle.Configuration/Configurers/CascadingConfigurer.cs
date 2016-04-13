using System;
using System.Collections.Generic;
using Mantle.Configuration.Interfaces;
using Mantle.Extensions;

namespace Mantle.Configuration.Configurers
{
    public class CascadingConfigurer<T> : BaseConfigurer<T>
    {
        private readonly IConfigurer<T>[] configurers;

        public CascadingConfigurer(params IConfigurer<T>[] configurers)
        {
            configurers.Require(nameof(configurers));

            if (configurers.Length == 0)
                throw new ArgumentException("You must supply at least one (1) configurer.", nameof(configurers));

            this.configurers = configurers;
        }

        public override IEnumerable<ConfigurationSetting> GetConfigurationSettings()
        {
            var settingDictionary = new Dictionary<string, ConfigurationSetting>();

            foreach (var configurer in configurers)
            {
                foreach (ConfigurationSetting setting in configurer.GetConfigurationSettings())
                {
                    settingDictionary[setting.Name] = setting;
                }
            }

            return settingDictionary.Values;
        }
    }
}