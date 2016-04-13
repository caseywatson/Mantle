using Amazon.SQS;
using Mantle.Aws.Interfaces;
using Mantle.Interfaces;
using Mantle.Messaging.Aws.Constants;
using System;
using System.Configuration;

namespace Mantle.Messaging.Aws.Channels
{
    public abstract class BaseAwsSqsChannel<T> : IDisposable
        where T : class
    {
        private readonly IAwsRegionEndpoints awsRegionEndpoints;

        private AmazonSQSClient awsSqsClient;

        protected BaseAwsSqsChannel(IAwsRegionEndpoints awsRegionEndpoints, ISerializer<T> serializer)
        {
            this.awsRegionEndpoints = awsRegionEndpoints;

            Serializer = serializer;
        }

        public abstract bool AutoSetup { get; set; }

        public abstract string AwsAccessKeyId { get; set; }
        public abstract string AwsSecretAccessKey { get; set; }
        public abstract string AwsRegionName { get; set; }
        public abstract string QueueName { get; set; }
        
        public string QueueUrl { get; private set; }

        public AmazonSQSClient AmazonSQSClient => GetAwsSqsClient();

        protected ISerializer<T> Serializer { get; }

        private AmazonSQSClient GetAwsSqsClient()
        {
            if (awsSqsClient == null)
            {
                var awsRegionEndpoint = awsRegionEndpoints.GetRegionEndpointByName(AwsRegionName);

                if (awsRegionEndpoint == null)
                    throw new ConfigurationErrorsException($"[{AwsRegionName}] is not a known AWS region.");

                awsSqsClient = new AmazonSQSClient(AwsAccessKeyId, AwsSecretAccessKey, awsRegionEndpoint);

                SetupQueue(awsSqsClient);       
            }

            return awsSqsClient;
        }

        private void SetupQueue(AmazonSQSClient amazonSqsClient)
        {
            QueueUrl = GetQueueUrlByName(amazonSqsClient, QueueName);

            if (QueueUrl == null)
            {
                if (AutoSetup)
                    QueueUrl = amazonSqsClient.CreateQueue(QueueName).QueueUrl;
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
            catch (AmazonSQSException sqsException) when (sqsException.ErrorCode == AwsSqsErrorCodes.QueueDoesNotExist)
            {
                return null;
            }
        }

        public void Dispose()
        {
            if (awsSqsClient != null)
                awsSqsClient.Dispose();
        }
    }
}
