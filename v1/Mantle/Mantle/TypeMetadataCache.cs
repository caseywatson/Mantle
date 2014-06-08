using System;
using System.Collections.Concurrent;
using Mantle.Interfaces;

namespace Mantle
{
    public class TypeMetadataCache : ITypeMetadataCache
    {
        private readonly ConcurrentDictionary<Type, TypeMetadata> typeMetadatas;

        public TypeMetadataCache()
        {
            typeMetadatas = new ConcurrentDictionary<Type, TypeMetadata>();
        }

        public TypeMetadata GetTypeMetadata(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (typeMetadatas.ContainsKey(type))
                return typeMetadatas[type];

            var typeMetadata = new TypeMetadata(type);

            typeMetadatas.TryAdd(type, typeMetadata);

            return typeMetadata;
        }
    }
}