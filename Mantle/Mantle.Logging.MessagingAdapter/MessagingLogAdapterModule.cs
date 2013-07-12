using Ninject.Modules;

namespace Mantle.Logging.MessagingAdapter
{
    public class MessagingLogAdapterModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogAdapter>().To<MessagingLogAdapter>().InTransientScope();
        }
    }
}