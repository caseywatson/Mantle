using Mantle.Configuration.Configurers;
using Mantle.Identity.Commands;
using Mantle.Identity.Subscribers;
using Mantle.Messaging.Interfaces;
using Mantle.Ninject;
using Mantle.PhotoGallery.PhotoProcessing.Commands;
using Mantle.PhotoGallery.Processor.Worker.Subscribers;
using Ninject.Modules;

namespace Mantle.PhotoGallery.Processor.Console.Mantle.Profiles.Default
{
    public class SubscriberModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISubscriber<CopyPhoto>>()
                .To<CopyPhotoSubscriber>()
                .InTransientScope()
                .ConfigureUsing(new AppSettingsConfigurer<CopyPhotoSubscriber>(),
                                new ConnectionStringsConfigurer<CopyPhotoSubscriber>());

            Bind<ISubscriber<SavePhoto>>()
                .To<SavePhotoSubscriber>()
                .InTransientScope()
                .ConfigureUsing(new AppSettingsConfigurer<SavePhotoSubscriber>(),
                                new ConnectionStringsConfigurer<SavePhotoSubscriber>());

            Bind<ISubscriber<CreateUser>>()
                .To<CreateUserSubscriber>()
                .InTransientScope()
                .ConfigureUsing(new AppSettingsConfigurer<CreateUserSubscriber>(),
                                new ConnectionStringsConfigurer<CreateUserSubscriber>());

            Bind<ISubscriber<DeleteUser>>()
                .To<DeleteUserSubscriber>()
                .InTransientScope()
                .ConfigureUsing(new AppSettingsConfigurer<DeleteUserSubscriber>(),
                                new ConnectionStringsConfigurer<DeleteUserSubscriber>());

            Bind<ISubscriber<UpdateUser>>()
                .To<UpdateUserSubscriber>()
                .InTransientScope()
                .ConfigureUsing(new AppSettingsConfigurer<UpdateUserSubscriber>(),
                                new ConnectionStringsConfigurer<UpdateUserSubscriber>());
        }
    }
}