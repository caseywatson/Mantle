using System;
using System.Configuration;

namespace Mantle.Aws
{
    public class AwsConfiguration : IAwsConfiguration
    {
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }

        public void Validate()
        {
            if (String.IsNullOrEmpty(AccessKey))
                throw new ConfigurationErrorsException("AWS access key is required.");

            if (String.IsNullOrEmpty(SecretKey))
                throw new ConfigurationErrorsException("AWS secret key is required.");
        }
    }
}