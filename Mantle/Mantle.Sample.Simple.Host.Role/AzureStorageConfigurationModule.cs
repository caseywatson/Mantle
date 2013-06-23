using Mantle.Azure;
using Ninject.Modules;

namespace Mantle.Samples.Simple.Host.Role
{
    public class AzureStorageConfigurationModule : NinjectModule
    {
        public override void Load()
        {
            // TODO: Setup your Azure storage connection string.

            Bind<IAzureStorageConfiguration>()
                .To<AzureStorageConfiguration>()
                .InSingletonScope()
                .OnActivation(c => c.Setup("Replace this with your Azure storage connection string."));
        }
    }
}