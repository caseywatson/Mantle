using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mantle.Configuration.Interfaces;

namespace Mantle.Configuration.Providers
{
    public class EnvironmentVariablesConfigurationProvider : IConfigurationProvider
    {
        public IEnumerable<ConfigurationSetting> GetConfigurationSettings()
        {
            return Environment.GetEnvironmentVariables()
                .OfType<DictionaryEntry>()
                .Select(de => new ConfigurationSetting(de.Key.ToString(), de.Value.ToString()));
        }
    }
}