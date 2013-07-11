using System;
using System.Diagnostics;
using Topshelf;
using Topshelf.HostConfigurators;

namespace Mantle.Hosting.WindowsServices
{
    public class WindowsServiceWorkerHost : BaseWorkerHost
    {
        private readonly Host host;

        public WindowsServiceWorkerHost(IDependencyResolver dependencyResolver,
                                        Action<HostConfigurator> serviceConfigurator) : base(dependencyResolver)
        {
            if (serviceConfigurator == null)
                throw new ArgumentNullException("serviceConfigurator");

            host = HostFactory.New(x =>
            {
                x.Service<IWorker>(y =>
                {
                    y.ConstructUsing(z => Worker);
                    y.WhenStarted(z => z.Start());
                    y.WhenStopped(z => z.Stop());
                });

                serviceConfigurator(x);
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