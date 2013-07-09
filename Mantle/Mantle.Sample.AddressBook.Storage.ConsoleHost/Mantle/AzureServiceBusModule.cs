using Mantle.Messaging;
using Mantle.Messaging.Azure;
using Ninject.Modules;

namespace Mantle.Sample.AddressBook.Storage.ConsoleHost.Mantle
{
    public class AzureServiceBusModule : NinjectModule
    {
        public override void Load()
        {
            LoadPublisherEndpoints();
            LoadSubscriberEndpoints();
        }

        private void LoadPublisherEndpoints()
        {
            // TODO: Setup publisher endpoints. 

            //Bind<IPublisherEndpoint>()
            //    .To<AzureServiceBusQueuePublisherEndpoint>()
            //    .InSingletonScope()
            //    .OnActivation(
            //        c => c.Configure("My Publisher Endpoint",
            //            "Replace this text with the name of your Azure service bus queue."));
        }

        private void LoadSubscriberEndpoints()
        {
            // TODO: Setup subscriber endpoints.

            //Bind<ISubscriberEndpoint>()
            //    .To<AzureServiceBusQueueSubscriberEndpoint>()
            //    .InSingletonScope()
            //    .OnActivation(
            //        c => c.Configure("My Subscriber Endpoint",
            //            "Replace this text with the name of your Azure service bus queue."));
        }
    }
}