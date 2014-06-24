using System;
using System.Linq;
using Mantle.Configuration.Attributes;
using Mantle.Extensions;

namespace Mantle.Configuration.Extensions
{
    public static class GenericExtensions
    {
        public static ConfigurableObject<T> ToConfigurableObject<T>(this T @object, string objectName = null)
        {
            var configurableObject = new ConfigurableObject<T>();

            configurableObject.Name = objectName;
            configurableObject.Target = @object;
            configurableObject.TypeMetadata = new TypeMetadata(typeof (T));

            foreach (PropertyMetadata propertyMetadata in configurableObject.TypeMetadata.Properties)
            {
                ConfigurableAttribute configurableAttribute =
                    propertyMetadata.Attributes.OfType<ConfigurableAttribute>().SingleOrDefault();

                if (configurableAttribute != null)
                {
                    var configurableProperty = new ConfigurableProperty();

                    configurableProperty.IsRequired = configurableAttribute.IsRequired;
                    configurableProperty.PropertyMetadata = propertyMetadata;

                    configurableProperty.SettingName = (String.IsNullOrEmpty(configurableAttribute.SettingName)
                        ? configurableObject.GetConventionalSettingName(propertyMetadata)
                        : configurableAttribute.SettingName.Merge(new {Name = objectName}));

                    configurableObject.Properties.Add(configurableProperty);
                }
            }

            return configurableObject;
        }
    }
}