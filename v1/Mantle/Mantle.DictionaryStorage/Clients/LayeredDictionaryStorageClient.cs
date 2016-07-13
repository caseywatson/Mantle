using System;
using System.Collections.Generic;
using System.Linq;
using Mantle.DictionaryStorage.Entities;
using Mantle.DictionaryStorage.Interfaces;
using Mantle.Extensions;
using Mantle.Interfaces;

namespace Mantle.DictionaryStorage.Clients
{
    public class LayeredDictionaryStorageClient<TEntity, TLayer1, TLayer2, TLayer3, TLayer4> :
        LayeredDictionaryStorageClient<TEntity>
        where TEntity : class, new()
        where TLayer1 : IDictionaryStorageClient<TEntity>
        where TLayer2 : IDictionaryStorageClient<TEntity>
        where TLayer3 : IDictionaryStorageClient<TEntity>
        where TLayer4 : IDictionaryStorageClient<TEntity>
    {
        public LayeredDictionaryStorageClient(IDependencyResolver dependencyResolver)
            : base(dependencyResolver.Get<TLayer1>(),
                   dependencyResolver.Get<TLayer2>(),
                   dependencyResolver.Get<TLayer3>(),
                   dependencyResolver.Get<TLayer4>())
        {
        }
    }

    public class LayeredDictionaryStorageClient<TEntity, TLayer1, TLayer2, TLayer3> :
        LayeredDictionaryStorageClient<TEntity>
        where TEntity : class, new()
        where TLayer1 : IDictionaryStorageClient<TEntity>
        where TLayer2 : IDictionaryStorageClient<TEntity>
        where TLayer3 : IDictionaryStorageClient<TEntity>
    {
        public LayeredDictionaryStorageClient(IDependencyResolver dependencyResolver)
            : base(dependencyResolver.Get<TLayer1>(),
                   dependencyResolver.Get<TLayer2>(),
                   dependencyResolver.Get<TLayer3>())
        {
        }
    }

    public class LayeredDictionaryStorageClient<TEntity, TLayer1, TLayer2> :
        LayeredDictionaryStorageClient<TEntity>
        where TEntity : class, new()
        where TLayer1 : IDictionaryStorageClient<TEntity>
        where TLayer2 : IDictionaryStorageClient<TEntity>
    {
        public LayeredDictionaryStorageClient(IDependencyResolver dependencyResolver)
            : base(dependencyResolver.Get<TLayer1>(),
                   dependencyResolver.Get<TLayer2>())
        {
        }
    }

    public class LayeredDictionaryStorageClient<TEntity> :
        IDictionaryStorageClient<TEntity>, IDisposable
        where TEntity : class, new()
    {
        public LayeredDictionaryStorageClient()
        {
        }

        public LayeredDictionaryStorageClient(params IDictionaryStorageClient<TEntity>[] layers)
        {
            layers.Require(nameof(layers));

            Layers = layers.ToList();
        }

        public LayeredDictionaryStorageClient(IEnumerable<IDictionaryStorageClient<TEntity>> layers)
        {
            layers.Require(nameof(layers));

            Layers = layers.ToList();
        }

        public IList<IDictionaryStorageClient<TEntity>> Layers { get; set; }

        public bool DeleteEntity(string entityId, string partitionId)
        {
            entityId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));

            return (Layers.Count(l => l.DeleteEntity(entityId, partitionId)) > 0);
        }

        public bool DeletePartition(string partitionId)
        {
            partitionId.Require(nameof(partitionId));

            return (Layers.Count(l => l.DeletePartition(partitionId)) > 0);
        }

        public bool DoesEntityExist(string entityId, string partitionId)
        {
            entityId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));

            return Layers.Any(l => l.DoesEntityExist(entityId, partitionId));
        }

        public void InsertOrUpdateDictionaryStorageEntities(IEnumerable<DictionaryStorageEntity<TEntity>> entities)
        {
            entities.Require(nameof(entities));

            foreach (var layer in Layers)
                layer.InsertOrUpdateDictionaryStorageEntities(entities);
        }

        public void InsertOrUpdateDictionaryStorageEntity(DictionaryStorageEntity<TEntity> entity)
        {
            entity.Require(nameof(entity));

            foreach (var layer in Layers)
                layer.InsertOrUpdateDictionaryStorageEntity(entity);
        }

        public IEnumerable<DictionaryStorageEntity<TEntity>> LoadAllDictionaryStorageEntities(string partitionId)
        {
            partitionId.Require(nameof(partitionId));

            for (var i = 0; i < Layers.Count; i++)
            {
                var entities = Layers[i].LoadAllDictionaryStorageEntities(partitionId).ToList();

                if (entities.Any())
                {
                    for (var ii = (i - 1); i > 0; i--)
                    {
                        Layers[ii].InsertOrUpdateDictionaryStorageEntities(entities);
                    }

                    foreach (var iLayerEntity in entities)
                        yield return iLayerEntity;

                    yield break;
                }
            }
        }

        public DictionaryStorageEntity<TEntity> LoadDictionaryStorageEntity(string entityId, string partitionId)
        {
            entityId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));

            for (var i = 0; i < Layers.Count; i++)
            {
                var entity = Layers[i].LoadDictionaryStorageEntity(entityId, partitionId);

                if (entity != null)
                {
                    for (var ii = (i - 1); i > 0; i--)
                    {
                        Layers[ii].InsertOrUpdateDictionaryStorageEntity(entity);
                    }

                    return entity;
                }
            }

            return null;
        }

        public void Dispose()
        {
            if (Layers != null)
            {
                foreach (var layer in Layers.OfType<IDisposable>())
                    layer.Dispose();
            }
        }
    }
}