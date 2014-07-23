using System.Collections.Generic;
using System.Linq;
using Mantle.Configuration.Interfaces;
using Mantle.Extensions;

namespace Mantle.Configuration.Providers
{
    public class AdHocConfigurationProvider : IConfigurationProvider
    {
        private readonly Dictionary<string, object> configurationDictionary;

        public AdHocConfigurationProvider(object configurationObject)
        {
            configurationObject.Require("configurationObject");
            configurationDictionary = configurationObject.ToDictionary();
        }

        public IEnumerable<ConfigurationSetting> GetConfigurationSettings()
        {
            return configurationDictionary.Select(cs => new ConfigurationSetting(cs.Key, cs.Value.ToString()));
        }
    }
}