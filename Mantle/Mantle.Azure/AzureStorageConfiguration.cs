using System.Configuration;

namespace Mantle.Azure
{
    public class AzureStorageConfiguration : IAzureStorageConfiguration
    {
        public string ConnectionString { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(ConnectionString))
                throw new ConfigurationErrorsException("Azure storage connection string is required.");
        }
    }
}