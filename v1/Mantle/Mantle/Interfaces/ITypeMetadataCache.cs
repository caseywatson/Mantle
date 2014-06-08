using System;

namespace Mantle.Interfaces
{
    public interface ITypeMetadataCache
    {
        TypeMetadata GetTypeMetadata(Type type);
    }
}