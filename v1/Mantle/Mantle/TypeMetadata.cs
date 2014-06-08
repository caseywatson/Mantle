using System;
using System.Collections.Generic;
using System.Linq;

namespace Mantle
{
    public class TypeMetadata
    {
        public TypeMetadata()
        {
            Attributes = new List<Attribute>();
            Properties = new List<PropertyMetadata>();
        }

        public TypeMetadata(Type type)
            : this()
        {
            if (type == null)
                throw new ArgumentNullException("type");

            Load(type);
        }

        public Type Type { get; set; }
        public List<Attribute> Attributes { get; set; }
        public List<PropertyMetadata> Properties { get; set; }

        private void Load(Type type)
        {
            Type = type;
            Attributes.AddRange(type.GetCustomAttributes(false).OfType<Attribute>());
            Properties.AddRange(type.GetProperties().Select(p => new PropertyMetadata(p)));
        }
    }
}