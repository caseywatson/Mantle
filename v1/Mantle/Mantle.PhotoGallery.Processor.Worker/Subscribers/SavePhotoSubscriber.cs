using Mantle.Messaging.Interfaces;
using Mantle.Messaging.Subscribers;
using Mantle.PhotoGallery.PhotoProcessing.Commands;
using Mantle.PhotoGallery.PhotoProcessing.Interfaces;

namespace Mantle.PhotoGallery.Processor.Worker.Subscribers
{
    public class SavePhotoSubscriber : BaseSubscriber<SavePhoto>
    {
        private readonly IPhotoMetadataRepository photoMetadataRepository;

        public SavePhotoSubscriber(IPhotoMetadataRepository photoMetadataRepository)
        {
            this.photoMetadataRepository = photoMetadataRepository;
        }

        public override void HandleMessage(IMessageContext<SavePhoto> messageContext)
        {
            photoMetadataRepository.InsertOrUpdatePhotoMetadata(messageContext.Message.PhotoMetadata);
        }
    }
}