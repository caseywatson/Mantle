using Mantle.Azure;
using Ninject.Modules;

namespace Mantle.Samples.Simple.Host.Role
{
    public class AzureServiceBusConfigurationModule : NinjectModule
    {
        public override void Load()
        {
            // TODO: Setup your Azure service bus connection string.

            Bind<IAzureServiceBusConfiguration>()
                .To<AzureServiceBusConfiguration>()
                .InSingletonScope()
                .OnActivation(c => c.Setup("Replace this with your Azure service bus connection string."));
        }
    }
}