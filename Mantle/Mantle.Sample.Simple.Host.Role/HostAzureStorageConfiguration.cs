using Mantle.Azure;

namespace Mantle.Sample.Simple.Host.Role
{
    public class HostAzureStorageConfiguration : AzureStorageConfiguration
    {
        public HostAzureStorageConfiguration()
        {
            // TODO: Update "ConnectionString" with your Azure storage connection string.
            // To obtain an Azure storage connection string, go to http://www.windowsazure.com.

            ConnectionString = "Your Azure Storage Connection String";
        }
    }
}