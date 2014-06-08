using Ninject.Modules;

namespace Mantle.Logging.MessagingAdapter
{
    public class MessagingLogAdapterModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILogAdapter>()
                .To<MessagingLogAdapter>()
                .InTransientScope()
                .OnActivation(c => c.Configure("Enter the name of the log publishing endpoint here."));
        }
    }
}