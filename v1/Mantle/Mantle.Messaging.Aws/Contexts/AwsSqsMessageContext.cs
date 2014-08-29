using Amazon.SQS.Model;
using Mantle.Extensions;
using Mantle.Messaging.Aws.Channels;
using Mantle.Messaging.Interfaces;

namespace Mantle.Messaging.Aws.Contexts
{
    public class AwsSqsMessageContext<T> : IMessageContext<T>
        where T : class
    {
        public AwsSqsMessageContext(BaseAwsSqsChannel<T> channel, T message, string messageReceiptHandle)
        {
            channel.Require("channel");
            message.Require("message");
            messageReceiptHandle.Require("messageReceiptHandle");

            Channel = channel;
            Message = message;
            MessageReceiptHandle = messageReceiptHandle;
        }

        public BaseAwsSqsChannel<T> Channel { get; private set; }

        public int? DeliveryCount
        {
            get { return null; }
        }

        public bool IsAbandoned { get; private set; }
        public bool IsCompleted { get; private set; }
        public bool IsDeadLettered { get; private set; }

        public T Message { get; private set; }
        public string MessageReceiptHandle { get; private set; }

        public bool TryToAbandon()
        {
            try
            {
                Channel.SqsClient.ChangeMessageVisibility(new ChangeMessageVisibilityRequest
                {
                    QueueUrl = Channel.QueueUrl,
                    ReceiptHandle = MessageReceiptHandle,
                    VisibilityTimeout = 0
                });

                return (IsAbandoned = true);
            }
            catch
            {
                return false;
            }
        }

        public bool TryToComplete()
        {
            try
            {
                Channel.SqsClient.DeleteMessage(new DeleteMessageRequest
                {
                    QueueUrl = Channel.QueueUrl,
                    ReceiptHandle = MessageReceiptHandle
                });

                return (IsCompleted = true);
            }
            catch
            {
                return false;
            }
        }

        public bool TryToDeadLetter()
        {
            return false;
        }

        public bool TryToRenewLock()
        {
            try
            {
                Channel.SqsClient.ChangeMessageVisibility(new ChangeMessageVisibilityRequest
                {
                    QueueUrl = Channel.QueueUrl,
                    ReceiptHandle = MessageReceiptHandle,
                    VisibilityTimeout = 60
                });

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}