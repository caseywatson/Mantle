using System.Collections.Generic;
using Mantle.DictionaryStorage.Extensions;
using Mantle.DictionaryStorage.Interfaces;
using Mantle.Extensions;
using Mantle.PhotoGallery.PhotoProcessing.Interfaces;
using Mantle.PhotoGallery.PhotoProcessing.Models;

namespace Mantle.PhotoGallery.PhotoProcessing.Repositories
{
    public class PhotoMetadataRepository : IPhotoMetadataRepository
    {
        private const string AllPhotosPartitionId = "allphotos";

        private readonly IDictionaryStorageClient<PhotoMetadata> dictionaryStorageClient;

        public PhotoMetadataRepository(IDictionaryStorageClient<PhotoMetadata> dictionaryStorageClient)
        {
            this.dictionaryStorageClient = dictionaryStorageClient;
        }

        public void InsertOrUpdatePhotoMetadata(PhotoMetadata photoMetadata)
        {
            photoMetadata.Require(nameof(photoMetadata));

            dictionaryStorageClient.InsertOrUpdateEntity(photoMetadata, photoMetadata.Id, photoMetadata.UserId);
            dictionaryStorageClient.InsertOrUpdateEntity(photoMetadata, photoMetadata.Id, AllPhotosPartitionId);
        }

        public bool DeletePhotoMetadata(PhotoMetadata photoMetadata)
        {
            photoMetadata.Require(nameof(photoMetadata));

            return (dictionaryStorageClient.DeleteEntity(photoMetadata.Id, photoMetadata.UserId) &&
                    dictionaryStorageClient.DeleteEntity(photoMetadata.Id, AllPhotosPartitionId));
        }

        public IEnumerable<PhotoMetadata> GetAllPhotoMetadata()
        {
            return dictionaryStorageClient.LoadAllEntities(AllPhotosPartitionId);
        }

        public IEnumerable<PhotoMetadata> GetAllPhotoMetadataByUser(string userId)
        {
            userId.Require(nameof(userId));

            return dictionaryStorageClient.LoadAllEntities(userId);
        }

        public PhotoMetadata GetPhotoMetadata(string photoId)
        {
            photoId.Require(nameof(photoId));

            return dictionaryStorageClient.LoadEntity(photoId, AllPhotosPartitionId);
        }
    }
}