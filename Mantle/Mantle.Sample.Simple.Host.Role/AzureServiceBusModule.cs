using Mantle.Messaging;
using Mantle.Messaging.Azure;
using Ninject.Modules;

namespace Mantle.Sample.Simple.Host.Role
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
            // TODO: Setup Azure service bus publisher endpoints.

            Bind<IPublisherEndpoint>()
                .To<AzureServiceBusQueuePublisherEndpoint>()
                .InTransientScope()
                .OnActivation(
                    c =>
                    c.Setup("SimpleWorkerOutput",
                            "Enter the name of the Azure service bus queue here."));
        }

        private void LoadSubscriberEndpoints()
        {
            // TODO: Setup Azure service bus subscriber endpoints.

            Bind<ISubscriberEndpoint>()
                .To<AzureServiceBusQueueSubscriberEndpoint>()
                .InTransientScope()
                .OnActivation(
                    c =>
                    c.Setup("SimpleWorkerInput",
                            "Enter the name of the Azure service bus queue here."));
        }
    }
}