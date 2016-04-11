using System.Diagnostics;
using Mantle.Extensions;
using Mantle.Hosting.Hosts;
using Mantle.Hosting.Interfaces;

namespace Mantle.Hosting.Azure
{
    public class AzureCloudServiceRoleWorkerHost : BaseWorkerHost
    {
        public AzureCloudServiceRoleWorkerHost(IWorker worker)
            : base(worker)
        {
        }

        protected override void OnErrorOccurred(string message)
        {
            message.Require(nameof(message));

            Trace.TraceError(message);
        }

        protected override void OnMessageOccurred(string message)
        {
            message.Require(nameof(message));

            Trace.TraceInformation(message);
        }
    }
}