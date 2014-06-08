using System.Collections.Generic;

namespace Mantle.Configuration
{
    public class ConfigurationTarget<T>
    {
        public ConfigurationTarget()
        {
            Properties = new List<ConfigurationTargetProperty>();
        }

        public List<ConfigurationTargetProperty> Properties { get; set; }
        public T Target { get; set; }
        public string Name { get; set; }
        public TypeMetadata TypeMetadata { get; set; }
    }
}