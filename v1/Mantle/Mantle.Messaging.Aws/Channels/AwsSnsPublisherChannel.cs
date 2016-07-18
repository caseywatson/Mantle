using System;
using System.Configuration;
using Amazon.SimpleNotificationService;
using Mantle.Aws.Interfaces;
using Mantle.Configuration.Attributes;
using Mantle.Extensions;
using Mantle.FaultTolerance.Interfaces;
using Mantle.Interfaces;
using Mantle.Messaging.Interfaces;

namespace Mantle.Messaging.Aws.Channels
{
    public class AwsSnsPublisherChannel<T> : IPublisherChannel<T>, IDisposable
        where T : class
    {
        private readonly IAwsRegionEndpoints awsRegionEndpoints;
        private readonly ISerializer<T> serializer;
        private readonly ITransientFaultStrategy transientFaultStrategy;

        private AmazonSimpleNotificationServiceClient awsSnsClient;

        public AwsSnsPublisherChannel(IAwsRegionEndpoints awsRegionEndpoints,
                                      ISerializer<T> serializer,
                                      ITransientFaultStrategy transientFaultStrategy)
        {
            this.awsRegionEndpoints = awsRegionEndpoints;
            this.serializer = serializer;
            this.transientFaultStrategy = transientFaultStrategy;
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

        public void Dispose()
        {
            awsSnsClient?.Dispose();
        }

        public void Publish(T message)
        {
            message.Require(nameof(message));

            transientFaultStrategy.Try(
                () => AmazonSimpleNotificationServiceClient.Publish(TopicArn, serializer.Serialize(message)));
        }

        private AmazonSimpleNotificationServiceClient GetAwsSnsClient()
        {
            if (awsSnsClient == null)
            {
                var awsRegionEndpoint = awsRegionEndpoints.GetRegionEndpointByName(AwsRegionName);

                if (awsRegionEndpoint == null)
                    throw new ConfigurationErrorsException($"[{AwsRegionName}] is not a known AWS region.");

                awsSnsClient = transientFaultStrategy.Try(
                    () => new AmazonSimpleNotificationServiceClient(AwsAccessKeyId, AwsSecretAccessKey,
                                                                    awsRegionEndpoint));
            }

            return awsSnsClient;
        }
    }
}