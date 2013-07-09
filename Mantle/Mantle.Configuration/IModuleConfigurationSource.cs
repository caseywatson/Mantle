using System.Collections.Generic;

namespace Mantle.Configuration
{
    public interface IModuleConfigurationSource
    {
        IEnumerable<IModuleConfiguration> LoadConfiguration(string groupName);
    }
}