using Mantle.Messaging;
using Mantle.Messaging.Msmq;
using Ninject.Modules;

namespace Mantle.Sample.Simple.Host.Console
{
    public class MsmqModule : NinjectModule
    {
        public override void Load()
        {
            LoadPublishers();
            LoadSubscribers();
        }

        private void LoadPublishers()
        {
            // TODO: Setup MSMQ publisher endpoints.

            Bind<IPublisherEndpoint>()
                .To<MsmqPublisherEndpoint>()
                .InTransientScope()
                .OnActivation(c => c.Setup("SimpleWorkerOutput", "Enter the queue path here."));
        }

        private void LoadSubscribers()
        {
            // TODO: Setup MSMQ subscriber endpoints;

            Bind<ISubscriberEndpoint>()
                .To<MsmqSubscriberEndpoint>()
                .InTransientScope()
                .OnActivation(c => c.Setup("SimpleWorkerInput", "Enter the queue path here."));
        }
    }
}