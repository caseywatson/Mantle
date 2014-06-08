using Ninject.Modules;

namespace Mantle.Logging
{
    public class LogModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ILog>().To<Log>().InTransientScope();
        }
    }
}