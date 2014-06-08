using System;

namespace Mantle.Configuration.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class ConfigurableAttribute : Attribute
    {
        public bool IsRequired { get; set; }
        public string SettingName { get; set; }
    }
}