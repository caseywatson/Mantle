using System.Collections.Generic;

namespace Mantle.Configuration.Interfaces
{
    public interface IConfigurer<T>
    {
        T Configure(T target, string targetName = null);
        IEnumerable<ConfigurationSetting> GetConfigurationSettings(ConfigurationTarget<T> target);
    }
}