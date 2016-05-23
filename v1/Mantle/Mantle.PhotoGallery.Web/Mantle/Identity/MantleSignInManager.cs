using System.Security.Claims;
using System.Threading.Tasks;
using Mantle.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace Mantle.PhotoGallery.Web.Mantle.Identity
{
    public class MantleSignInManager : SignInManager<MantleUser, string>
    {
        public MantleSignInManager(MantleUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(MantleUser user)
        {
            return user.GenerateUserIdentityAsync((MantleUserManager) UserManager);
        }

        public static MantleSignInManager Create(IdentityFactoryOptions<MantleSignInManager> options,
                                                 IOwinContext context)
        {
            return new MantleSignInManager(context.GetUserManager<MantleUserManager>(), context.Authentication);
        }
    }
}