using Mantle.Configuration.Configurers;
using Mantle.Identity;
using Mantle.Identity.Azure.Repositories;
using Mantle.Identity.Interfaces;
using Mantle.Ninject;
using Ninject.Modules;

namespace Mantle.PhotoGallery.Processor.Console.Mantle.Profiles.Azure
{
    public class IdentityModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IMantleUserRepository<MantleUser>>()
                .To<DocumentDbMantleUserRepository>()
                .InTransientScope()
                .ConfigureUsing(new ConnectionStringsConfigurer<DocumentDbMantleUserRepository>(),
                                new AppSettingsConfigurer<DocumentDbMantleUserRepository>());
        }
    }
}