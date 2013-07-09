using Ninject.Modules;

namespace Mantle.Sample.AddressBook.Storage.ConsoleHost.Mantle
{
    public class AzureStorageQueueModule : NinjectModule
    {
        public override void Load()
        {
            LoadPublisherEndpoints();
            LoadSubscriberEndpoints();
        }

        private void LoadPublisherEndpoints()
        {
            // TODO: Setup publisher endpoints.

            //Bind<ISubscriberEndpoint>()
            //    .To<AzureStorageQueueSubscriberEndpoint>()
            //    .InSingletonScope()
            //    .OnActivation(
            //        c => c.Configure("My Publisher Endpoint", "Replace this text with the name of your Azure storage queue."));
        }

        private void LoadSubscriberEndpoints()
        {
            // TODO: Setup subscriber endpoints.

            //Bind<ISubscriberEndpoint>()
            //    .To<AzureStorageQueueSubscriberEndpoint>()
            //    .InSingletonScope()
            //    .OnActivation(
            //        c => c.Configure("My Subscriber Endpoint", "Replace this text with the name of your Azure storage queue."));
        }
    }
}