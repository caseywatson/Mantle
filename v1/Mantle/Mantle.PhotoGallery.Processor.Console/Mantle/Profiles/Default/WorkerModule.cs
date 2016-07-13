using Mantle.Hosting.Interfaces;
using Mantle.PhotoGallery.Processor.Worker.Workers;
using Ninject.Modules;

namespace Mantle.PhotoGallery.Processor.Console.Mantle.Profiles.Default
{
    public class WorkerModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IWorker>()
                .To<ProcessorWorker>()
                .InTransientScope();
        }
    }
}