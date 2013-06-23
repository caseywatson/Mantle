using System.Diagnostics;
using System.Reflection;
using Mantle.Hosting;
using Ninject;
using Topshelf;

namespace Mantle.Samples.Simple.Host.WindowsService
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            HostFactory.Run(x =>
                {
                    x.Service<IWorker>(s =>
                        {
                            s.ConstructUsing(c => CreateWorker());
                            s.WhenStarted(c => c.Start());
                            s.WhenStopped(c => c.Stop());
                        });

                    x.RunAsLocalSystem();

                    x.SetDescription("This is a simple example of the Mantle framework.");
                    x.SetDisplayName("Mantle Simple Example Service");
                    x.SetServiceName("MantleSimpleExampleService");
                });
        }

        private static IWorker CreateWorker()
        {
            var kernel = new StandardKernel();

            kernel.Load(Assembly.GetExecutingAssembly());

            var worker = kernel.Get<IWorker>();

            worker.ErrorOccurred += m => Trace.TraceError(m);
            worker.MessageOccurred += m => Trace.TraceInformation(m);

            return worker;
        }
    }
}