using System;
using System.IO;
using Mantle.BlobStorage.Interfaces;
using Mantle.Configuration.Attributes;
using Mantle.Extensions;
using Mantle.Interfaces;
using Mantle.PhotoGallery.PhotoProcessing.Interfaces;

namespace Mantle.PhotoGallery.PhotoProcessing.Services
{
    public class PhotoStorageService : IPhotoStorageService
    {
        private readonly IDirectory<IBlobStorageClient> blobStorageClientDirectory;

        private IBlobStorageClient blobStorageClient;

        public PhotoStorageService(IDirectory<IBlobStorageClient> blobStorageClientDirectory)
        {
            this.blobStorageClientDirectory = blobStorageClientDirectory;
        }

        [Configurable(IsRequired = true)]
        public string BlobStorageClientName { get; set; }

        public MemoryStream LoadPhoto(string photoId)
        {
            photoId.Require(nameof(photoId));

            return null;
        }

        public void SavePhoto(string photoId, MemoryStream photoStream)
        {
            throw new NotImplementedException();
        }

        private IBlobStorageClient GetBlobStorageClient()
        {
            return (blobStorageClient = (blobStorageClient ??
                                         blobStorageClientDirectory[BlobStorageClientName]));
        }
    }
}