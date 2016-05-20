using System.Reflection;

namespace Mantle.Configuration.Interfaces
{
    public interface IPropertyConfigurer
    {
        bool CanConfigureProperty(PropertyInfo propertyInfo);

        void Configure<T>(ConfigurableObject<T> cfgObject, ConfigurableProperty cfgProperty,
                          ConfigurationSetting cfgSetting);
    }
}