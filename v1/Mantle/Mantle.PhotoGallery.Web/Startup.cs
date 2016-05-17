using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Mantle.PhotoGallery.Web.Startup))]
namespace Mantle.PhotoGallery.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
