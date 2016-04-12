using Amazon.SQS;
using Amazon.SQS.Model;
using Mantle.Extensions;
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
        private readonly AmazonSQSClient sqsClient;

        public AwsSqsMessageContext(AmazonSQSClient sqsClient, Message sqsMessage, T message)
        {
            sqsClient.Require(nameof(sqsClient));
            sqsMessage.Require(nameof(sqsMessage));
            message.Require(nameof(message));

            this.sqsClient = sqsClient;

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
            throw new NotImplementedException();
        }

        public bool TryToComplete()
        {
            throw new NotImplementedException();
        }

        public bool TryToDeadLetter()
        {
            throw new NotImplementedException();
        }

        public bool TryToRenewLock()
        {
            throw new NotImplementedException();
        }
    }
}
