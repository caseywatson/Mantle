using Mantle.Hosting.Messaging.Workers;
using Mantle.Identity.Commands;
using Mantle.Interfaces;
using Mantle.Messaging.Interfaces;
using Mantle.Messaging.Messages;
using Mantle.PhotoGallery.PhotoProcessing.Commands;
using Mantle.PhotoGallery.Processor.Worker.Constants;

namespace Mantle.PhotoGallery.Processor.Worker.Workers
{
    public class ProcessorWorker : SubscriptionWorker
    {
        public ProcessorWorker(IDependencyResolver dependencyResolver) : base(dependencyResolver)
        {
            AddSubscriberChannel(
                dependencyResolver.Get<ISubscriberChannel<MessageEnvelope>>(
                    ChannelNames.ProcessorChannel));

            SubscribeTo<CopyPhoto>();
            SubscribeTo<SavePhoto>();

            SubscribeTo<CreateUser>();
            SubscribeTo<DeleteUser>();
            SubscribeTo<UpdateUser>();
        }
    }
}