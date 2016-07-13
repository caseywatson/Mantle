using System;
using Mantle.BlobStorage.Interfaces;
using Mantle.Configuration.Attributes;
using Mantle.Interfaces;
using Mantle.Messaging.Interfaces;
using Mantle.Messaging.Subscribers;
using Mantle.PhotoGallery.PhotoProcessing.Commands;
using Mantle.PhotoGallery.PhotoProcessing.Interfaces;
using Mantle.PhotoGallery.PhotoProcessing.Models;

namespace Mantle.PhotoGallery.Processor.Worker.Subscribers
{
    public class SavePhotoSubscriber : BaseSubscriber<SavePhoto>
    {
        private readonly IDirectory<IBlobStorageClient> blobStorageDirectory;
        private readonly IPhotoMetadataRepository photoMetadataRepository;
        private readonly IPhotoThumbnailService photoThumbnailService;

        private IBlobStorageClient photoStorageClient;
        private IBlobStorageClient thumbnailStorageClient;

        public SavePhotoSubscriber(IDirectory<IBlobStorageClient> blobStorageDirectory,
                                   IPhotoMetadataRepository photoMetadataRepository,
                                   IPhotoThumbnailService photoThumbnailService)
        {
            this.blobStorageDirectory = blobStorageDirectory;
            this.photoMetadataRepository = photoMetadataRepository;
            this.photoThumbnailService = photoThumbnailService;
        }

        [Configurable(IsRequired = true)]
        public string PhotoDestination { get; set; }

        [Configurable(IsRequired = true)]
        public string ThumbnailDestination { get; set; }

        public override void HandleMessage(IMessageContext<SavePhoto> messageContext)
        {
            var photoMetadata = messageContext.Message.PhotoMetadata;

            CreateThumbnail(photoMetadata);
            photoMetadataRepository.InsertOrUpdatePhotoMetadata(photoMetadata);
        }

        private void CreateThumbnail(PhotoMetadata photoMetadata)
        {
            var photo = GetPhotoStorageClient().DownloadBlob(photoMetadata.Id);

            if (photo == null)
                throw new InvalidOperationException(
                    $"Photo [{photoMetadata.Id}] not found. Unable to create thumbnail image.");

            var thumbnail = photoThumbnailService.GenerateThumbnail(photo);

            GetThumbnailStorageClient().UploadBlob(thumbnail, photoMetadata.Id);
        }

        private IBlobStorageClient GetPhotoStorageClient()
        {
            return (photoStorageClient = (photoStorageClient ??
                                          blobStorageDirectory[PhotoDestination]));
        }

        private IBlobStorageClient GetThumbnailStorageClient()
        {
            return (thumbnailStorageClient = (thumbnailStorageClient ??
                                              blobStorageDirectory[ThumbnailDestination]));
        }
    }
}