using System.Collections.Generic;

namespace Mantle.Configuration
{
    public class ModuleConfiguration : IModuleConfiguration
    {
        public ModuleConfiguration()
        {
            Properties = new Dictionary<string, string>();
        }

        public string Name { get; set; }
        public string GroupName { get; set; }

        public Dictionary<string, string> Properties { get; set; }
    }
}