using System;
using System.Collections.Generic;
using System.Linq;
using Mantle.DictionaryStorage.Entities;
using Mantle.DictionaryStorage.Interfaces;
using Mantle.Extensions;

namespace Mantle.DictionaryStorage.Clients
{
    public class LayeredDictionaryStorageClient<T> : IDictionaryStorageClient<T>, IDisposable
        where T : class, new()
    {
        public LayeredDictionaryStorageClient()
        {
            Layers = new List<IDictionaryStorageClient<T>>();
        }

        public IList<IDictionaryStorageClient<T>> Layers { get; set; }

        public void DeleteEntity(string entityId, string partitionId)
        {
            entityId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));

            foreach (var layer in Layers)
                layer.DeleteEntity(entityId, partitionId);
        }

        public bool DoesEntityExist(string entityId, string partitionId)
        {
            entityId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));

            return Layers.Any(l => l.DoesEntityExist(entityId, partitionId));
        }

        public void InsertOrUpdateDictionaryStorageEntities(IEnumerable<DictionaryStorageEntity<T>> entities)
        {
            entities.Require(nameof(entities));

            foreach (var layer in Layers)
                layer.InsertOrUpdateDictionaryStorageEntities(entities);
        }

        public void InsertOrUpdateDictionaryStorageEntity(DictionaryStorageEntity<T> entity)
        {
            entity.Require(nameof(entity));

            foreach (var layer in Layers)
                layer.InsertOrUpdateDictionaryStorageEntity(entity);
        }

        public IEnumerable<DictionaryStorageEntity<T>> LoadAllDictionaryStorageEntities(string partitionId)
        {
            partitionId.Require(nameof(partitionId));

            for (var i = 0; i < Layers.Count; i++)
            {
                var iLayer = Layers[i];
                var iLayerDsEntities = iLayer.LoadAllDictionaryStorageEntities(partitionId).ToList();

                if (iLayerDsEntities.Any())
                {
                    if (i > 0)
                    {
                        for (var ii = i; ii >= 0; ii--)
                        {
                            var iiLayer = Layers[ii];

                            iiLayer.InsertOrUpdateDictionaryStorageEntities(iLayerDsEntities);
                        }
                    }

                    foreach (var iLayerDsEntity in iLayerDsEntities)
                        yield return iLayerDsEntity;
                }
            }
        }

        public DictionaryStorageEntity<T> LoadDictionaryStorageEntity(string entityId, string partitionId)
        {
            entityId.Require(nameof(entityId));
            partitionId.Require(nameof(partitionId));

            for (var i = 0; i < Layers.Count; i++)
            {
                var iLayer = Layers[i];
                var iLayerDsEntity = iLayer.LoadDictionaryStorageEntity(entityId, partitionId);

                if (iLayerDsEntity != null)
                {
                    if (i > 0)
                    {
                        for (var ii = i; ii >= 0; ii--)
                        {
                            var iiLayer = Layers[ii];

                            iiLayer.InsertOrUpdateDictionaryStorageEntity(iLayerDsEntity);
                        }
                    }

                    return iLayerDsEntity;
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