using Mantle.Messaging.Interfaces;
using Mantle.Messaging.Strategies;
using Ninject.Modules;

namespace Mantle.Sample.PublisherConsole.Mantle.Profiles.Default
{
    public class MessagingModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof (IDeadLetterStrategy<>)).To(typeof (DefaultDeadLetterStrategy<>)).InTransientScope();
        }
    }
}