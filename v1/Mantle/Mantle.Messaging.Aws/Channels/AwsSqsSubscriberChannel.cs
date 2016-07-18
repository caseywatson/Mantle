using System;
using System.Linq;
using System.Threading.Tasks;
using Amazon.SQS.Model;
using Mantle.Aws.Interfaces;
using Mantle.Configuration.Attributes;
using Mantle.FaultTolerance.Interfaces;
using Mantle.Interfaces;
using Mantle.Messaging.Aws.Constants;
using Mantle.Messaging.Aws.Contexts;
using Mantle.Messaging.Interfaces;

namespace Mantle.Messaging.Aws.Channels
{
    public class AwsSqsSubscriberChannel<T> : BaseAwsSqsChannel<T>, ISubscriberChannel<T>
        where T : class
    {
        private readonly ITransientFaultStrategy transientFaultStrategy;

        public AwsSqsSubscriberChannel(IAwsRegionEndpoints awsRegionEndpoints,
                                       ISerializer<T> serializer,
                                       ITransientFaultStrategy transientFaultStrategy)
            : base(awsRegionEndpoints, serializer, transientFaultStrategy)
        {
            this.transientFaultStrategy = transientFaultStrategy;

            DefaultMessageReceiveTimeout = TimeSpan.FromSeconds(20);
            MessageVisibilityTimeout = TimeSpan.FromSeconds(30);
        }

        [Configurable]
        public override bool AutoSetup { get; set; }

        [Configurable(IsRequired = true)]
        public override string AwsAccessKeyId { get; set; }

        [Configurable(IsRequired = true)]
        public override string AwsRegionName { get; set; }

        [Configurable(IsRequired = true)]
        public override string AwsSecretAccessKey { get; set; }

        [Configurable]
        public TimeSpan DefaultMessageReceiveTimeout { get; set; }

        [Configurable]
        public TimeSpan MessageVisibilityTimeout { get; set; }

        [Configurable(IsRequired = true)]
        public override string QueueName { get; set; }

        public IMessageContext<T> Receive(TimeSpan? timeout = null)
        {
            timeout = (timeout ?? DefaultMessageReceiveTimeout);

            var sqsClient = AmazonSqsClient;

            var receiveMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = QueueUrl,
                WaitTimeSeconds = timeout.Value.Seconds,
                VisibilityTimeout = ((int) (MessageVisibilityTimeout.TotalSeconds))
            };

            receiveMessageRequest.AttributeNames.Add(AwsSqsMessageAttributes.ApproximateReceiveCount);

            var receiveMessageResponse = transientFaultStrategy.Try(
                () => sqsClient.ReceiveMessage(receiveMessageRequest));

            var message = receiveMessageResponse.Messages?.FirstOrDefault();

            if (message == null)
                return null;

            return new AwsSqsMessageContext<T>(
                message,
                Serializer.Deserialize(message.Body),
                TryToAbandonMessage,
                TryToCompleteMessage,
                m => false,
                TryToRenewMessageLock);
        }

        public Task<IMessageContext<T>> ReceiveAsync()
        {
            throw new NotImplementedException();
        }

        private bool TryToAbandonMessage(Message sqsMessage)
        {
            try
            {
                transientFaultStrategy.Try(
                    () => AmazonSqsClient.ChangeMessageVisibility(QueueUrl, sqsMessage.ReceiptHandle, 0));

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool TryToCompleteMessage(Message sqsMessage)
        {
            try
            {
                transientFaultStrategy.Try(
                    () => AmazonSqsClient.DeleteMessage(QueueUrl, sqsMessage.ReceiptHandle));

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool TryToRenewMessageLock(Message sqsMessage)
        {
            try
            {
                transientFaultStrategy.Try(
                    () => AmazonSqsClient.ChangeMessageVisibility(
                        QueueUrl,
                        sqsMessage.ReceiptHandle,
                        ((int) (MessageVisibilityTimeout.TotalSeconds))));

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}