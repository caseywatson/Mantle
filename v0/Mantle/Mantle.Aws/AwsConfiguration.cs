using System;
using System.Configuration;
using Mantle.Configuration;

namespace Mantle.Aws
{
    public class AwsConfiguration : IAwsConfiguration, IConfigurable
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

        public void Configure(IConfigurationMetadata metadata)
        {
            if (metadata == null)
                throw new ArgumentNullException("metadata");

            if (metadata.Properties.ContainsKey(ConfigurationProperties.AccessKey))
                AccessKey = metadata.Properties[ConfigurationProperties.AccessKey];

            if (metadata.Properties.ContainsKey(ConfigurationProperties.SecretKey))
                SecretKey = metadata.Properties[ConfigurationProperties.SecretKey];

            Validate();
        }

        public void Configure(string accessKey, string secretKey)
        {
            AccessKey = accessKey;
            SecretKey = secretKey;

            Validate();
        }

        public static class ConfigurationProperties
        {
            public const string AccessKey = "AccessKey";
            public const string SecretKey = "SecretKey";
        }
    }
}