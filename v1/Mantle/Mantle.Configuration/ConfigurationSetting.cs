using Mantle.Extensions;

namespace Mantle.Configuration
{
    public class ConfigurationSetting
    {
        public ConfigurationSetting()
        {
        }

        public ConfigurationSetting(string name, string value)
        {
            name.Require("name");

            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}