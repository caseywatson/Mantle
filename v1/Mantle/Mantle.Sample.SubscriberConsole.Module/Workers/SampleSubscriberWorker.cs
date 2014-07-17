using Mantle.Hosting.Messaging.Workers;
using Mantle.Interfaces;
using Mantle.Messaging.Interfaces;
using Mantle.Messaging.Messages;
using Mantle.Sample.SubscriberConsole.Module.Models;

namespace Mantle.Sample.SubscriberConsole.Module.Workers
{
    public class SampleSubscriberWorker : SubscriptionWorker
    {
        public SampleSubscriberWorker(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        {
            UseSubscriberChannel(dependencyResolver.Get<ISubscriberChannel<MessageEnvelope>>("AzServiceBusQueue"));

            SubscribeTo<SampleModel>();
        }
    }
}