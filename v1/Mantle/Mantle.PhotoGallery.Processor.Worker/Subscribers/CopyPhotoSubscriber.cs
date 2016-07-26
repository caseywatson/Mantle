using System;
using Mantle.Configuration.Attributes;
using Mantle.Interfaces;
using Mantle.Messaging.Extensions;
using Mantle.Messaging.Interfaces;
using Mantle.Messaging.Messages;
using Mantle.Messaging.Subscribers;
using Mantle.PhotoGallery.PhotoProcessing.Commands;
using Mantle.PhotoGallery.PhotoProcessing.Interfaces;
using Mantle.PhotoGallery.Processor.Worker.Constants;

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

            SaveImageCommandChannelName = ChannelNames.SaveImageCommandChannel;
        }

        [Configurable(IsRequired = true)]
        public string PhotoDestination { get; set; }

        [Configurable]
        public string SaveImageCommandChannelName { get; set; }

        public override void HandleMessage(IMessageContext<CopyPhoto> messageContext)
        {
            var message = messageContext.Message;

            try
            {
                if (message.PhotoSource != PhotoDestination)
                {
                    OnMessageOccurred(messageContext,
                                      $"[{nameof(CopyPhotoSubscriber)}]: Copying photo [{message.PhotoMetadata.Id}] from " +
                                      $"[{message.PhotoSource}] to [{PhotoDestination}]...");

                    photoCopyService.CopyPhoto(message.PhotoMetadata, message.PhotoSource, PhotoDestination);
                    messageContext.TryToRenewLock();

                    OnMessageOccurred(messageContext,
                                      $"[{nameof(CopyPhotoSubscriber)}]: Copied photo [{message.PhotoMetadata.Id}] from " +
                                      $"[{message.PhotoSource}] to [{PhotoDestination}]. Sending [SavePhoto] command...");

                    GetPublisherChannel().Publish(new SavePhoto(message.PhotoMetadata));

                    OnMessageOccurred(messageContext,
                                      $"[{nameof(CopyPhotoSubscriber)}]: Photo [{message.PhotoMetadata.Id}] [SavePhoto] command sent.");
                }
            }
            catch (Exception ex)
            {
                OnErrorOccurred(messageContext,
                                $"[{nameof(CopyPhotoSubscriber)}]: An error occurred while processing a message: [{ex.Message}]");

                throw;
            }
        }

        private IPublisherChannel<MessageEnvelope> GetPublisherChannel()
        {
            return (publisherChannel = (publisherChannel ??
                                        publisherChannelDirectory[SaveImageCommandChannelName]));
        }
    }
}