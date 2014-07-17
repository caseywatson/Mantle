using Mantle.Hosting.Interfaces;
using Mantle.Sample.PublisherConsole.Module.Workers;
using Ninject.Modules;

namespace Mantle.Sample.PublisherConsole.Mantle.Profiles.Default
{
    public class WorkerModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IWorker>().To<SamplePublisherWorker>().InSingletonScope();
        }
    }
}