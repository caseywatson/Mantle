using Amazon.SimpleNotificationService;
using Mantle.Aws.Interfaces;
using Mantle.Configuration.Attributes;
using Mantle.Extensions;
using Mantle.Interfaces;
using Mantle.Messaging.Interfaces;
using System;
using System.Configuration;

namespace Mantle.Messaging.Aws.Channels
{
    public class AwsSnsPublisherChannel<T> : IPublisherChannel<T>, IDisposable
        where T : class
    {
        private readonly IAwsRegionEndpoints awsRegionEndpoints;
        private readonly ISerializer<T> serializer;

        private AmazonSimpleNotificationServiceClient awsSnsClient;

        public AwsSnsPublisherChannel(IAwsRegionEndpoints awsRegionEndpoints, ISerializer<T> serializer)
        {
            this.awsRegionEndpoints = awsRegionEndpoints;
            this.serializer = serializer;
        }

        [Configurable(IsRequired = true)]
        public string AwsAccessKeyId { get; set; }

        [Configurable(IsRequired = true)]
        public string AwsRegionName { get; set; }

        [Configurable(IsRequired = true)]
        public string AwsSecretAccessKey { get; set; }

        [Configurable(IsRequired = true)]
        public string TopicArn { get; set; }

        public AmazonSimpleNotificationServiceClient AmazonSimpleNotificationServiceClient => GetAwsSnsClient();

        public void Publish(T message)
        {
            message.Require(nameof(message));

            AmazonSimpleNotificationServiceClient.Publish(TopicArn, serializer.Serialize(message));
        }

        private AmazonSimpleNotificationServiceClient GetAwsSnsClient()
        {
            if (awsSnsClient == null)
            {
                var awsRegionEndpoint = awsRegionEndpoints.GetRegionEndpointByName(AwsRegionName);

                if (awsRegionEndpoint == null)
                    throw new ConfigurationErrorsException($"[{AwsRegionName}] is not a known AWS region.");

                awsSnsClient = new AmazonSimpleNotificationServiceClient(AwsAccessKeyId, AwsSecretAccessKey, awsRegionEndpoint);
            }

            return awsSnsClient;
        }

        public void Dispose()
        {
            if (awsSnsClient != null)
                awsSnsClient.Dispose();
        }
    }
}
