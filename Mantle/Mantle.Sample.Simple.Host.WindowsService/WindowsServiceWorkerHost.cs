using System.Diagnostics;
using Mantle.Hosting;
using Topshelf;

namespace Mantle.Samples.Simple.Host.WindowsService
{
    public class WindowsServiceWorkerHost : BaseWorkerHost
    {
        private Topshelf.Host host;

        public WindowsServiceWorkerHost(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        {
        }

        protected override void Setup()
        {
            base.Setup();

            host = HostFactory.New(x =>
                {
                    x.Service<IWorker>(s =>
                        {
                            s.ConstructUsing(c => Worker);
                            s.WhenStarted(c => c.Start());
                            s.WhenStopped(c => c.Stop());
                        });

                    x.RunAsLocalSystem();

                    x.SetDescription("This is a simple example of the Mantle framework.");
                    x.SetDisplayName("Mantle Simple Example Service");
                    x.SetServiceName("MantleSimpleExampleService");
                });
        }

        public override void Start()
        {
            host.Run();
        }

        protected override void OnErrorOccurred(string message)
        {
            Trace.TraceError(message);
        }

        protected override void OnMessageOccurred(string message)
        {
            Trace.TraceInformation(message);
        }
    }
}