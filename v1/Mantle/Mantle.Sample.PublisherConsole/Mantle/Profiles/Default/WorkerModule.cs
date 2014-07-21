using Mantle.Hosting.Interfaces;
using Ninject.Modules;

namespace Mantle.Sample.PublisherConsole.Mantle.Profiles.Default
{
    public class WorkerModule : NinjectModule
    {
        public override void Load()
        {
            // TODO: Wire up your worker class from the module project here.
            // Bind<IWorker>().To<SamplePublisherWorker>().InSingletonScope();
        }
    }
}