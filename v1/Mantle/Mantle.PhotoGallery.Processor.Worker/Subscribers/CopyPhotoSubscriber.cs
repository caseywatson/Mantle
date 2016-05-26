using Mantle.Configuration.Attributes;
using Mantle.Interfaces;
using Mantle.Messaging.Extensions;
using Mantle.Messaging.Interfaces;
using Mantle.Messaging.Messages;
using Mantle.Messaging.Subscribers;
using Mantle.PhotoGallery.PhotoProcessing.Commands;
using Mantle.PhotoGallery.PhotoProcessing.Interfaces;

namespace Mantle.PhotoGallery.Processor.Worker.Subscribers
{
    public class CopyPhotoSubscriber : BaseSubscriber<CopyPhoto>
    {
        private readonly IPhotoCopyService photoCopyService;
        private readonly IDirectory<IPublisherChannel<MessageEnvelope>> publisherChannelDirectory;

        private IPublisherChannel<MessageEnvelope> publisherChannel;

        public CopyPhotoSubscriber(IPhotoCopyService photoCopyService,
                                   IDirectory<IPublisherChannel<MessageEnvelope>> publisherChannelDirectory)
        {
            this.photoCopyService = photoCopyService;
            this.publisherChannelDirectory = publisherChannelDirectory;
        }

        [Configurable(IsRequired = true)]
        public string PhotoDestination { get; set; }

        [Configurable]
        public string SaveImageCommandChannelName { get; set; }

        public override void HandleMessage(IMessageContext<CopyPhoto> messageContext)
        {
            var message = messageContext.Message;

            if (message.PhotoSource != PhotoDestination)
            {
                photoCopyService.CopyPhoto(message.PhotoMetadata, message.PhotoSource, PhotoDestination);
                GetPublisherChannel().Publish(new SavePhoto(message.PhotoMetadata));
            }
        }

        private IPublisherChannel<MessageEnvelope> GetPublisherChannel()
        {
            return (publisherChannel = (publisherChannel ??
                                        publisherChannelDirectory[SaveImageCommandChannelName]));
        }
    }
}