using System;

namespace Mantle.Configuration.Extensions
{
    public static class ConfigurableObjectExtensions
    {
        private static string GetConventionalSettingName<T>(this ConfigurableObject<T> configurableObject,
                                                            PropertyMetadata propertyMetadata)
        {
            if (String.IsNullOrEmpty(configurableObject.Name))
            {
                return String.Format("{0}.{1}", configurableObject.TypeMetadata.Type.Name,
                                     propertyMetadata.PropertyInfo.Name);
            }

            return String.Format("{0}.{1}.{2}", configurableObject.Name, configurableObject.TypeMetadata.Type.Name,
                                 propertyMetadata.PropertyInfo.Name);
        }
    }
}