using Mantle.Azure;

namespace Mantle.Sample.Simple.Host.Role
{
    public class HostAzureServiceBusConfiguration : AzureServiceBusConfiguration
    {
        public HostAzureServiceBusConfiguration()
        {
            // TODO: Update "ConnectionString" with your Azure service bus connection string.
            // To obtain an Azure service bus connection string, go to http://www.windowsazure.com.

            ConnectionString = "Your Azure Service Bus Connection String";
        }
    }
}