using System;
using System.Collections.Generic;
using System.Reflection;

namespace Mantle
{
    public class PropertyMetadata
    {
        public PropertyMetadata()
        {
            Attributes = new List<Attribute>();
        }

        public PropertyMetadata(PropertyInfo propertyInfo)
            : this()
        {
            if (propertyInfo == null)
                throw new ArgumentNullException("propertyInfo");

            Load(propertyInfo);
        }

        public List<Attribute> Attributes { get; set; }
        public PropertyInfo PropertyInfo { get; set; }

        private void Load(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
            Attributes.AddRange(propertyInfo.GetCustomAttributes());
        }
    }
}