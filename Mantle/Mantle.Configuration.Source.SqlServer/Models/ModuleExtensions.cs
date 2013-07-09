using System;
using System.Configuration;

namespace Mantle.Configuration.Source.SqlServer.Models
{
    public static class ModuleExtensions
    {
        public static ConfigurationMetadata ToModuleConfiguration(this Module module)
        {
            if (module == null)
                throw new ArgumentNullException("module");

            var configuration = new ConfigurationMetadata {GroupName = module.Group.Name, Name = module.Name};

            foreach (PropertyValue property in module.PropertyValues)
            {
                if (configuration.Properties.ContainsKey(property.Property.Name))
                    throw new ConfigurationErrorsException(
                        String.Format("Module [{0}] configuration property [{1}] is defined more than once.",
                            configuration.Name, property.Property.Name));

                configuration.Properties[property.Property.Name] = property.Value;
            }

            return configuration;
        }
    }
}