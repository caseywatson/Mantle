namespace Mantle.Configuration
{
    public class ConfigurationTargetProperty
    {
        public bool IsRequired { get; set; }
        public PropertyMetadata PropertyMetadata { get; set; }
        public string SettingName { get; set; }
    }
}