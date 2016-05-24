using Mantle.Identity.Commands;
using Mantle.Identity.Subscribers;
using Mantle.Messaging.Interfaces;
using Ninject.Modules;

namespace Mantle.PhotoGallery.Processor.Worker.Modules
{
    public class SubscriberModule : NinjectModule
    {
        public override void Load()
        {
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