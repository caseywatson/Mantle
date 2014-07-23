using System.Collections.Generic;

namespace Mantle.Configuration.Interfaces
{
    public interface IConfigurationProvider
    {
        IEnumerable<ConfigurationSetting> GetConfigurationSettings();
    }
}