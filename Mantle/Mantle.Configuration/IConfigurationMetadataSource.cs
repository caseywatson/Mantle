using System.Collections.Generic;

namespace Mantle.Configuration
{
    public interface IConfigurationMetadataSource
    {
        IEnumerable<IConfigurationMetadata> LoadConfiguration(string groupName);
    }
}