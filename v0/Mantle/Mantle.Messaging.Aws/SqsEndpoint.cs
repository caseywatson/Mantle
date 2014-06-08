using System;
using Mantle.Configuration;

namespace Mantle.Messaging.Aws
{
    public abstract class SqsEndpoint : Endpoint, IConfigurable
    {
        public string QueueUrl { get; set; }

        public void Configure(IConfigurationMetadata metadata)
        {
            if (metadata == null)
                throw new ArgumentNullException("metadata");

            Name = metadata.Name;

            if (metadata.Properties.ContainsKey(ConfigurationProperties.QueueUrl))
                QueueUrl = metadata.Properties[ConfigurationProperties.QueueUrl];

            Validate();
        }

        public void Configure(string name, string queueUrl)
        {
            Name = name;
            QueueUrl = queueUrl;

            Validate();
        }

        public override void Validate()
        {
            base.Validate();

            if (String.IsNullOrEmpty(QueueUrl))
                throw new MessagingException("SQS queue URL is required.");
        }

        public static class ConfigurationProperties
        {
            public const string QueueUrl = "QueueUrl";
        }
    }
}