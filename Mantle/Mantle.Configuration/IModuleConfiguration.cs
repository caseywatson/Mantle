using System.Collections.Generic;

namespace Mantle.Configuration
{
    public interface IModuleConfiguration
    {
        string Name { get; }
        string GroupName { get; }

        Dictionary<string, string> Properties { get; }
    }
}