using Mantle.Configuration.Configurers;
using Mantle.Identity;
using Mantle.Identity.Aws.Repositories;
using Mantle.Identity.Interfaces;
using Mantle.Identity.Services;
using Mantle.Ninject;
using Ninject.Modules;

namespace Mantle.PhotoGallery.Web.Mantle.Profiles.AwsDeployment
{
    public class IdentityModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMantleUserCommandService<MantleUser>>()
                .To<DynamoDbMantleUserRepository>()
                .InTransientScope()
                .ConfigureUsing(new AppSettingsConfigurer<DynamoDbMantleUserRepository>());

            Bind<IMantleUserCommandService<MantleUser>>()
                .To<ChannelMantleUserCommandService>()
                .InTransientScope()
                .ConfigureUsing(new AppSettingsConfigurer<ChannelMantleUserCommandService>());

            Bind<IMantleUserQueryService<MantleUser>>()
                .To<DynamoDbMantleUserRepository>()
                .InTransientScope()
                .ConfigureUsing(new AppSettingsConfigurer<DynamoDbMantleUserRepository>());
        }
    }
}