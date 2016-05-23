using Mantle.Identity;
using Mantle.Identity.Interfaces;
using Mantle.Identity.Services;
using Microsoft.AspNet.Identity;
using Ninject.Modules;

namespace Mantle.PhotoGallery.Web.Mantle.Profiles.Default
{
    public class IdentityModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUserStore<MantleUser>>()
                .To<MantleUserStore<MantleUser>>()
                .InTransientScope();

            Bind<IMantleUserService<MantleUser>>()
                .To<CqrsMantleUserService>()
                .InTransientScope();
        }
    }
}