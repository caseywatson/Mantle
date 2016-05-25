using Mantle.PhotoGallery.PhotoProcessing.Interfaces;
using Mantle.PhotoGallery.PhotoProcessing.Repositories;
using Ninject.Modules;

namespace Mantle.PhotoGallery.Web.Mantle.Profiles.Default
{
    public class RepositoryModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPhotoMetadataRepository>()
                .To<PhotoMetadataRepository>()
                .InTransientScope();
        }
    }
}