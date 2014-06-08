using System.Diagnostics;

namespace Mantle.Hosting.Azure
{
    public class AzureRoleWorkerHost : BaseWorkerHost
    {
        public AzureRoleWorkerHost(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        {
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