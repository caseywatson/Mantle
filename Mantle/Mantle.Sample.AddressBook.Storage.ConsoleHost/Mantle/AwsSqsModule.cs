using Mantle.Messaging;
using Mantle.Messaging.Aws;
using Ninject.Modules;

namespace Mantle.Sample.AddressBook.Storage.ConsoleHost.Mantle
{
    public class AwsSqsModule : NinjectModule
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
                .To<SqsSubscriberEndpoint>()
                .InSingletonScope()
                .OnActivation(
                    c => c.Setup(
                        "PersonQueue",
                        "Replace this text with your SQS queue URL."));
        }
    }
}