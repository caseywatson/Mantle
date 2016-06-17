using System;
using System.Collections.Generic;
using System.Linq;
using Mantle.Interfaces;

namespace Mantle
{
    public class TypeMetadata<T> : TypeMetadata, ITypeMetadata<T>
    {
        public TypeMetadata()
            : base(typeof(T))
        {
        }
    }

    public class TypeMetadata : ITypeMetadata
    {
        public TypeMetadata(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            Type = type;
            Attributes = type.GetCustomAttributes(false).OfType<Attribute>().ToList();
            Properties = type.GetProperties().Select(p => new PropertyMetadata(p)).ToList();
        }

        public IList<Attribute> Attributes { get; set; }
        public IList<PropertyMetadata> Properties { get; set; }
        public Type Type { get; set; }
    }
}