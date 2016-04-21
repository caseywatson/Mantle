using System.Collections.Generic;
using System.Linq;
using Mantle.Extensions;

namespace Mantle.Configuration.Configurers
{
    public class AdHocConfigurer<T> : BaseConfigurer<T>
    {
        private readonly Dictionary<string, object> configurationDictionary;

        public AdHocConfigurer(object configurationObject)
        {
            configurationObject.Require(nameof(configurationObject));
            configurationDictionary = configurationObject.ToDictionary();
        }

        public override IEnumerable<ConfigurationSetting> GetConfigurationSettings()
        {
            return configurationDictionary.Select(cs => new ConfigurationSetting(cs.Key, cs.Value.ToString()));
        }
    }
}