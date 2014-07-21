using System.Linq;
using System.Net;
using System.Reflection;
using Mantle;
using Mantle.Hosting.Azure;
using Mantle.Hosting.Interfaces;
using Mantle.Ninject;
using Microsoft.WindowsAzure.ServiceRuntime;
using Ninject;

namespace WorkerRole1
{
    public class WorkerRole : RoleEntryPoint
    {
        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }

        public override void Run()
        {
            var kernel =
                new StandardKernel(Assembly.GetExecutingAssembly().LoadConfiguredProfileMantleModules().ToArray());

            var dependencyResolver = new NinjectDependencyResolver(kernel);

            MantleContext.Current = new MantleContext();
            MantleContext.Current.DependencyResolver = dependencyResolver;

            var workerHost = new AzureCloudServiceRoleWorkerHost(dependencyResolver.Get<IWorker>());

            workerHost.Start();
        }
    }
}