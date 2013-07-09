using System.Net;
using System.Reflection;
using Mantle.Hosting;
using Mantle.Hosting.Azure;
using Mantle.Ninject;
using Microsoft.WindowsAzure.ServiceRuntime;
using Ninject;

namespace Mantle.Sample.AddressBook.Storage.Host
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly IWorkerHost host;

        public WorkerRole()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            host = new AzureRoleWorkerHost(new NinjectDependencyResolver(kernel));
        }

        public override void Run()
        {
            host.Start();
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }
    }
}