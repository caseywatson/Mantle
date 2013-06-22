using System;
using System.Configuration;

namespace Mantle.Azure
{
    public class AzureStorageConfiguration : IAzureStorageConfiguration
    {
        public string ConnectionString { get; set; }

        public void Setup(string connectionString)
        {
            ConnectionString = connectionString;

            Validate();
        }

        public void Validate()
        {
            if (String.IsNullOrEmpty(ConnectionString))
                throw new ConfigurationErrorsException("Azure storage connection string is required.");
        }
    }
}