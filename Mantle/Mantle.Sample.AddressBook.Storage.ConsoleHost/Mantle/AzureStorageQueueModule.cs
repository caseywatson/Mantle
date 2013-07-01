using Mantle.Messaging;
using Mantle.Messaging.Azure;
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
        }

        private void LoadSubscriberEndpoints()
        {
            // TODO: Setup subscriber endpoints.

            Bind<ISubscriberEndpoint>()
                .To<AzureStorageQueueSubscriberEndpoint>()
                .InSingletonScope()
                .OnActivation(
                    c => c.Setup("PersonQueue", "Replace this text with the name of your Azure storage queue."));
        }
    }
}