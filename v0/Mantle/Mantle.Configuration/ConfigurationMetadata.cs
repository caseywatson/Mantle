using System.Collections.Generic;

namespace Mantle.Configuration
{
    public class ConfigurationMetadata : IConfigurationMetadata
    {
        public ConfigurationMetadata()
        {
            Properties = new Dictionary<string, string>();
        }

        public string Name { get; set; }
        public string GroupName { get; set; }

        public Dictionary<string, string> Properties { get; set; }
    }
}