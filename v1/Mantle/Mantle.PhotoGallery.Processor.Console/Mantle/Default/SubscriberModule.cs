using Mantle.Identity.Commands;
using Mantle.Identity.Subscribers;
using Mantle.Messaging.Interfaces;
using Mantle.PhotoGallery.PhotoProcessing.Commands;
using Mantle.PhotoGallery.Processor.Worker.Subscribers;
using Ninject.Modules;

namespace Mantle.PhotoGallery.Processor.Console.Mantle.Default
{
    public class SubscriberModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISubscriber<CopyPhoto>>()
                .To<CopyPhotoSubscriber>()
                .InTransientScope();

            Bind<ISubscriber<SavePhoto>>()
                .To<SavePhotoSubscriber>()
                .InTransientScope();

            Bind<ISubscriber<CreateUser>>()
                .To<CreateUserSubscriber>()
                .InTransientScope();

            Bind<ISubscriber<DeleteUser>>()
                .To<DeleteUserSubscriber>()
                .InTransientScope();

            Bind<ISubscriber<UpdateUser>>()
                .To<UpdateUserSubscriber>()
                .InTransientScope();
        }
    }
}