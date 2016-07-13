using Mantle.Configuration.Configurers;
using Mantle.Identity;
using Mantle.Identity.Aws.Repositories;
using Mantle.Identity.Interfaces;
using Mantle.Ninject;
using Ninject.Modules;

namespace Mantle.PhotoGallery.Processor.Console.Mantle.Profiles.Aws
{
    public class IdentityModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMantleUserRepository<MantleUser>>()
                .To<DynamoDbMantleUserRepository>()
                .InTransientScope()
                .ConfigureUsing(new ConnectionStringsConfigurer<DynamoDbMantleUserRepository>(),
                                new AppSettingsConfigurer<DynamoDbMantleUserRepository>());
        }
    }
}