using Mantle.Azure;
using Ninject.Modules;

namespace Mantle.Sample.AddressBook.Storage.ConsoleHost.Mantle
{
    public class AzureConfigurationModule : NinjectModule
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
                        "Endpoint=sb://mantletest.servicebus.windows.net/;SharedSecretIssuer=owner;SharedSecretValue=UQxITa659SF4fTCSTPC+dHKrRAQHyiIpaJpv4R180rE="));

            // TODO: Setup your Azure storage configuration.

            Bind<IAzureStorageConfiguration>()
                .To<AzureStorageConfiguration>()
                .InSingletonScope()
                .OnActivation(
                    c =>
                    c.Setup(
                        "DefaultEndpointsProtocol=http;AccountName=mantleteststorage;AccountKey=v5u4RW5TeR5epUfcjt+aPEseezGx2YgHqA/LboV9N+hgzNv2aLzPJn2g1uiGr+LHjA/6/duhx5bEfXbSjH9RaA=="));
        }
    }
}