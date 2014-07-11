using Amazon;
using Amazon.SQS;
using Mantle.Interfaces;

namespace Mantle.Messaging.Aws.Channels
{
    public abstract class BaseAwsSqsChannel<T>
        where T : class
    {
        private IAmazonSQS sqsClient;

        protected BaseAwsSqsChannel(ISerializer<T> serializer)
        {
            Serializer = serializer;
        }

        public abstract string AwsAccessKey { get; set; }
        public abstract string AwsSecretAccessKey { get; set; }
        public abstract string QueueUrl { get; set; }

        public IAmazonSQS SqsClient
        {
            get { return GetSqsClient(); }
        }

        protected ISerializer<T> Serializer { get; private set; }

        private IAmazonSQS GetSqsClient()
        {
            return (sqsClient = (sqsClient ??
                                 AWSClientFactory.CreateAmazonSQSClient(AwsAccessKey, AwsSecretAccessKey)));
        }
    }
}