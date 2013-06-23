using System.Diagnostics;
using Topshelf;
using Topshelf.HostConfigurators;

namespace Mantle.Hosting.WindowsServices
{
    public abstract class WindowsServiceWorkerHost : BaseWorkerHost
    {
        private Host host;

        protected WindowsServiceWorkerHost(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        {
        }

        public override void Start()
        {
            host.Run();
        }

        protected override void Setup()
        {
            host = HostFactory.New(x =>
                {
                    x.Service<IWorker>(s =>
                        {
                            s.ConstructUsing(c => Worker);
                            s.WhenStarted(c => c.Start());
                            s.WhenStopped(c => c.Stop());
                        });

                    SetupService(x);
                });
        }

        protected abstract void SetupService(HostConfigurator configuration);

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