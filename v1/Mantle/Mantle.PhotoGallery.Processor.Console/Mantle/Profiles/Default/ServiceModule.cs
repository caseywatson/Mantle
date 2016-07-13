using Mantle.Configuration.Configurers;
using Mantle.Ninject;
using Mantle.PhotoGallery.PhotoProcessing.Interfaces;
using Mantle.PhotoGallery.PhotoProcessing.Services;
using Ninject.Modules;

namespace Mantle.PhotoGallery.Processor.Console.Mantle.Profiles.Default
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPhotoCopyService>()
                .To<PhotoCopyService>()
                .InTransientScope()
                .ConfigureUsing(new AppSettingsConfigurer<PhotoCopyService>());

            Bind<IPhotoThumbnailService>()
                .To<PhotoThumbnailService>()
                .InTransientScope()
                .ConfigureUsing(new AppSettingsConfigurer<PhotoThumbnailService>());
        }
    }
}