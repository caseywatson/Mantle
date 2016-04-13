using Amazon.SQS;
using Amazon.SQS.Model;
using Mantle.Extensions;
using Mantle.Messaging.Aws.Channels;
using Mantle.Messaging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantle.Messaging.Aws.Contexts
{
    public class AwsSqsMessageContext<T> : IMessageContext<T>
        where T : class
    {
        private readonly BaseAwsSqsChannel<T> sqsChannel;

        public AwsSqsMessageContext(BaseAwsSqsChannel<T> sqsChannel, Message sqsMessage, T message)
        {
            sqsChannel.Require(nameof(sqsChannel));
            sqsMessage.Require(nameof(sqsMessage));
            message.Require(nameof(message));

            this.sqsChannel = sqsChannel;

            SqsMessage = sqsMessage;
            Message = message;
        }

        public int? DeliveryCount { get; private set; }

        public bool IsAbandoned { get; private set; }
        public bool IsCompleted { get; private set; }
        public bool IsDeadLettered { get; private set; }

        public Message SqsMessage { get; private set; }

        public T Message { get; private set; }

        public bool TryToAbandon()
        {
            if (IsAbandoned)
                return true;

            try
            {
                sqsChannel.AwsSqsClient.ChangeMessageVisibility(sqsChannel.QueueUrl, SqsMessage.ReceiptHandle, 0);
                return (IsAbandoned = true);
            }
            catch
            {
                return false;
            }
        }

        public bool TryToComplete()
        {
            if (IsCompleted)
                return true;

            try
            {
                sqsChannel.AwsSqsClient.DeleteMessage(sqsChannel.QueueUrl, SqsMessage.ReceiptHandle);
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
            throw new NotImplementedException();
        }
    }
}
