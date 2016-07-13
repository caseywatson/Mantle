using Mantle.Configuration.Configurers;
using Mantle.Ninject;
using Mantle.PhotoGallery.Web.Interfaces;
using Mantle.PhotoGallery.Web.Metadata;
using Ninject.Modules;

namespace Mantle.PhotoGallery.Web.Mantle.Profiles.Default
{
    public class DeploymentModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDeploymentMetadata>()
                .To<DeploymentMetadata>()
                .InTransientScope()
                .ConfigureUsing(new AppSettingsConfigurer<DeploymentMetadata>());
        }
    }
}