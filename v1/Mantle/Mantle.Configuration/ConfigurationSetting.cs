using System;

namespace Mantle.Configuration
{
    public class ConfigurationSetting
    {
        public ConfigurationSetting()
        {
        }

        public ConfigurationSetting(string name, string value)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentException("Name is required.", "name");

            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}