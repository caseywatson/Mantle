using System;
using System.Configuration;
using Amazon.SQS;
using Amazon.SQS.Model;
using Mantle.Aws.Interfaces;
using Mantle.FaultTolerance.Interfaces;
using Mantle.Interfaces;

namespace Mantle.Messaging.Aws.Channels
{
    public abstract class BaseAwsSqsChannel<T> : IDisposable
        where T : class
    {
        private readonly IAwsRegionEndpoints awsRegionEndpoints;
        private readonly ITransientFaultStrategy transientFaultStrategy;

        private AmazonSQSClient awsSqsClient;

        protected BaseAwsSqsChannel(IAwsRegionEndpoints awsRegionEndpoints,
                                    ISerializer<T> serializer,
                                    ITransientFaultStrategy transientFaultStrategy)
        {
            this.awsRegionEndpoints = awsRegionEndpoints;
            this.transientFaultStrategy = transientFaultStrategy;

            Serializer = serializer;
        }

        public abstract bool AutoSetup { get; set; }

        public abstract string AwsAccessKeyId { get; set; }
        public abstract string AwsSecretAccessKey { get; set; }
        public abstract string AwsRegionName { get; set; }
        public abstract string QueueName { get; set; }

        public string QueueUrl { get; private set; }

        public AmazonSQSClient AmazonSqsClient => GetAwsSqsClient();

        protected ISerializer<T> Serializer { get; }

        public void Dispose()
        {
            awsSqsClient?.Dispose();
        }

        private AmazonSQSClient GetAwsSqsClient()
        {
            if (awsSqsClient == null)
            {
                var awsRegionEndpoint = awsRegionEndpoints.GetRegionEndpointByName(AwsRegionName);

                if (awsRegionEndpoint == null)
                    throw new ConfigurationErrorsException($"[{AwsRegionName}] is not a known AWS region.");

                awsSqsClient = transientFaultStrategy.Try(
                    () => new AmazonSQSClient(AwsAccessKeyId, AwsSecretAccessKey, awsRegionEndpoint));

                SetupQueue(awsSqsClient);
            }

            return awsSqsClient;
        }

        private void SetupQueue(AmazonSQSClient amazonSqsClient)
        {
            QueueUrl = transientFaultStrategy.Try(() => GetQueueUrlByName(amazonSqsClient, QueueName));

            if (QueueUrl == null)
            {
                if (AutoSetup)
                    QueueUrl = transientFaultStrategy.Try(() => amazonSqsClient.CreateQueue(QueueName).QueueUrl);
                else
                    throw new ConfigurationErrorsException($"AWS SQS queue [{QueueName}] does not exist.");
            }
        }

        private string GetQueueUrlByName(AmazonSQSClient amazonSqsClient, string queueName)
        {
            try
            {
                return amazonSqsClient.GetQueueUrl(queueName).QueueUrl;
            }
            catch (QueueDoesNotExistException)
            {
                return null;
            }
        }
    }
}