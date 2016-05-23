using Mantle.Configuration.Configurers;
using Mantle.Identity;
using Mantle.Identity.Azure.Repositories;
using Mantle.Identity.Interfaces;
using Mantle.Identity.Services;
using Mantle.Ninject;
using Ninject.Modules;

namespace Mantle.PhotoGallery.Web.Mantle.Profiles.AzureDeployment
{
    public class IdentityModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMantleUserCommandService<MantleUser>>()
                .To<DocumentDbMantleUserRepository>()
                .InTransientScope()
                .ConfigureUsing(new AppSettingsConfigurer<DocumentDbMantleUserRepository>());

            Bind<IMantleUserCommandService<MantleUser>>()
                .To<ChannelMantleUserCommandService>()
                .InTransientScope()
                .ConfigureUsing(new AppSettingsConfigurer<ChannelMantleUserCommandService>());

            Bind<IMantleUserQueryService<MantleUser>>()
                .To<DocumentDbMantleUserRepository>()
                .InTransientScope()
                .ConfigureUsing(new AppSettingsConfigurer<DocumentDbMantleUserRepository>());
        }
    }
}