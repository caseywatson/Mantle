using Mantle.Messaging;
using Ninject.Modules;

namespace Mantle.Sample.AddressBook.Storage.ConsoleHost.Mantle
{
    public class MessagingModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPublisherEndpointDirectory>().To<PublisherEndpointDirectory>().InSingletonScope();
            Bind<ISubscriberEndpointDirectory>().To<SubscriberEndpointDirectory>().InSingletonScope();
        }
    }
}
