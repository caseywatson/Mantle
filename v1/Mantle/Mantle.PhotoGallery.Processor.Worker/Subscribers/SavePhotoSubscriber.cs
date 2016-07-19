using System;
using Mantle.BlobStorage.Interfaces;
using Mantle.Configuration.Attributes;
using Mantle.Interfaces;
using Mantle.Messaging.Interfaces;
using Mantle.Messaging.Subscribers;
using Mantle.PhotoGallery.PhotoProcessing.Commands;
using Mantle.PhotoGallery.PhotoProcessing.Interfaces;

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
            try
            {
                var photoMetadata = messageContext.Message.PhotoMetadata;

                CreateThumbnail(messageContext);
                messageContext.TryToRenewLock();

                OnMessageOccurred(messageContext,
                                  $"[{nameof(SavePhotoSubscriber)}]: Updating photo [{photoMetadata.Id}] metadata...");

                photoMetadataRepository.InsertOrUpdatePhotoMetadata(photoMetadata);

                OnMessageOccurred(messageContext,
                                  $"[{nameof(SavePhotoSubscriber)}]: Photo [{photoMetadata.Id}] updated.");
            }
            catch (Exception ex)
            {
                OnErrorOccurred(messageContext,
                                $"[{nameof(SavePhotoSubscriber)}]: An error occurred while processing a message: [{ex.Message}]");

                throw;
            }
        }

        private void CreateThumbnail(IMessageContext<SavePhoto> messageContext)
        {
            var photoMetadata = messageContext.Message.PhotoMetadata;

            var photo = GetPhotoStorageClient().DownloadBlob(photoMetadata.Id);

            if (photo == null)
            {
                throw new InvalidOperationException($"Photo [{photoMetadata.Id}] not found. " +
                                                    "Unable to create thumbnail image.");
            }

            OnMessageOccurred(messageContext,
                              $"[{nameof(SavePhotoSubscriber)}]: Creating photo [{photoMetadata.Id}] thumbnail...");

            var thumbnail = photoThumbnailService.GenerateThumbnail(photo);

            OnMessageOccurred(messageContext,
                              $"[{nameof(SavePhotoSubscriber)}]: Created photo [{photoMetadata.Id}] thumbnail. " +
                              "Saving thumbnail...");

            GetThumbnailStorageClient().UploadBlob(thumbnail, photoMetadata.Id);

            OnMessageOccurred(messageContext,
                              $"[{nameof(SavePhotoSubscriber)}]: Saved photo [{photoMetadata.Id}] thumbnail.");
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