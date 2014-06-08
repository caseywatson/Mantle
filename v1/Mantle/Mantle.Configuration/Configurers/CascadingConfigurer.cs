using System;
using System.Collections.Generic;
using Mantle.Configuration.Interfaces;
using Mantle.Interfaces;

namespace Mantle.Configuration.Configurers
{
    public class CascadingConfigurer<T> : BaseConfigurer<T>
    {
        private readonly IConfigurer<T>[] configurers;

        public CascadingConfigurer(params IConfigurer<T>[] configurers)
        {
            if (configurers == null)
                throw new ArgumentNullException("configurers");

            this.configurers = configurers;
        }

        public CascadingConfigurer(ITypeMetadataCache typeMetadataCache, params IConfigurer<T>[] configurers)
            : base(typeMetadataCache)
        {
            if (configurers == null)
                throw new ArgumentNullException("configurers");

            this.configurers = configurers;
        }

        public override IEnumerable<ConfigurationSetting> GetConfigurationSettings(ConfigurationTarget<T> target)
        {
            var settingDictionary = new Dictionary<string, ConfigurationSetting>();

            foreach (var configurer in configurers)
            {
                foreach (ConfigurationSetting setting in configurer.GetConfigurationSettings(target))
                {
                    settingDictionary[setting.Name] = setting;
                }
            }

            return settingDictionary.Values;
        }
    }
}