using Mantle.Hosting;
using Mantle.Samples.Simple.Worker;
using Ninject.Modules;

namespace Mantle.Sample.Simple.Host.WindowsService
{
    public class WorkerModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IWorker>().To<SimpleWorker>().InSingletonScope();
        }
    }
}