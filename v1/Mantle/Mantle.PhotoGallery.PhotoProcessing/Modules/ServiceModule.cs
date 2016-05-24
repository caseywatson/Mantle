using Mantle.PhotoGallery.PhotoProcessing.Interfaces;
using Mantle.PhotoGallery.PhotoProcessing.Services;
using Ninject.Modules;

namespace Mantle.PhotoGallery.PhotoProcessing.Modules
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPhotoThumbnailService>()
                .To<PhotoThumbnailService>()
                .InTransientScope();
        }
    }
}