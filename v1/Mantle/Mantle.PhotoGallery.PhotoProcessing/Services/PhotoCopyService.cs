using Mantle.BlobStorage.Interfaces;
using Mantle.Extensions;
using Mantle.Interfaces;
using Mantle.PhotoGallery.PhotoProcessing.Interfaces;
using Mantle.PhotoGallery.PhotoProcessing.Models;

namespace Mantle.PhotoGallery.PhotoProcessing.Services
{
    public class PhotoCopyService : IPhotoCopyService
    {
        private readonly IDirectory<IBlobStorageClient> blobStorageClientDirectory;

        public PhotoCopyService(IDirectory<IBlobStorageClient> blobStorageClientDirectory)
        {
            this.blobStorageClientDirectory = blobStorageClientDirectory;
        }

        public void CopyPhoto(PhotoMetadata photoMetadata, string photoSource, string photoDestination)
        {
            photoMetadata.Require(nameof(photoMetadata));
            photoSource.Require(nameof(photoSource));
            photoDestination.Require(nameof(photoDestination));

            var sourceBlobStorageClient = blobStorageClientDirectory[photoSource];
            var destinationBlobStorageClient = blobStorageClientDirectory[photoDestination];
            var photoStream = sourceBlobStorageClient.DownloadBlob(photoMetadata.Id);

            photoStream.TryToRewind();

            destinationBlobStorageClient.UploadBlob(photoStream, photoMetadata.Id);
        }
    }
}