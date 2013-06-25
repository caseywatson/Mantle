using Mantle.Azure;
using Ninject.Modules;

namespace Mantle.Sample.AddressBook.Web.Mantle
{
    public class AzureServiceBusConfigurationModule : NinjectModule
    {
        public override void Load()
        {
            // TODO: Setup your Azure service bus configuration.

            Bind<IAzureServiceBusConfiguration>()
                .To<AzureServiceBusConfiguration>()
                .InSingletonScope()
                .OnActivation(
                    c =>
                    c.Setup(
                        "Replace this text with your Azure service bus connection string."));
        }
    }
}