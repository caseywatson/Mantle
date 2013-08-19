using System;
using Mantle.Aws;
using Mantle.Configuration;

namespace Mantle.Messaging.Aws
{
    public class SnsPublisherEndpoint : Endpoint, IPublisherEndpoint
    {
        private readonly IAwsConfiguration awsConfiguration;

        public SnsPublisherEndpoint(IAwsConfiguration awsConfiguration)
        {
            if (awsConfiguration == null)
                throw new ArgumentNullException("awsConfiguration");

            awsConfiguration.Validate();

            this.awsConfiguration = awsConfiguration;
        }

        public string TopicArn { get; set; }

        public override void Validate()
        {
            base.Validate();

            if (String.IsNullOrEmpty(TopicArn))
                throw new MessagingException("SNS topic ARN is required.");
        }

        public IPublisherClient GetClient()
        {
            return new SnsPublisherClient(this, awsConfiguration);
        }

        public IPublisherEndpointManager GetManager()
        {
            return new SnsPublisherEndpointManager(this, awsConfiguration);
        }

        public void Configure(IConfigurationMetadata metadata)
        {
            if (metadata == null)
                throw new ArgumentNullException("metadata");

            Name = metadata.Name;

            if (metadata.Properties.ContainsKey(ConfigurationProperties.TopicArn))
                TopicArn = metadata.Properties[ConfigurationProperties.TopicArn];

            Validate();
        }

        public void Configure(string name, string topicArn)
        {
            Name = name;
            TopicArn = topicArn;

            Validate();
        }

        public static class ConfigurationProperties
        {
            public const string TopicArn = "TopicArn";
        }
    }
}