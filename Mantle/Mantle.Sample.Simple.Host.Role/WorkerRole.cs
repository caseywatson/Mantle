using System.Net;
using System.Reflection;
using Mantle.Hosting;
using Microsoft.WindowsAzure.ServiceRuntime;
using Ninject;

namespace Mantle.Samples.Simple.Host.Role
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly IWorker worker;

        public WorkerRole()
        {
            var kernel = new StandardKernel();

            kernel.Load(Assembly.GetExecutingAssembly());

            worker = kernel.Get<IWorker>();
        }

        public override void Run()
        {
            worker.Start();
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