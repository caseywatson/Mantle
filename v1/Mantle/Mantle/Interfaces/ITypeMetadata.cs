using System;
using System.Collections.Generic;

namespace Mantle.Interfaces
{
    public interface ITypeMetadata<T> : ITypeMetadata
    {
    }

    public interface ITypeMetadata
    {
        IList<Attribute> Attributes { get; }
        IList<PropertyMetadata> Properties { get; }
        Type Type { get; }
    }
}