using System;
using Amazon.SQS.Model;
using Mantle.Extensions;
using Mantle.Messaging.Aws.Constants;
using Mantle.Messaging.Interfaces;

namespace Mantle.Messaging.Aws.Contexts
{
    public class AwsSqsMessageContext<T> : IMessageContext<T>
        where T : class
    {
        private readonly Func<Message, bool> tryAbandonMessage;
        private readonly Func<Message, bool> tryCompleteMessage;
        private readonly Func<Message, bool> tryDeadLetterMessage;
        private readonly Func<Message, bool> tryRenewMessageLock;

        public AwsSqsMessageContext(Message sqsMessage, T message,
                                    Func<Message, bool> tryAbandonMessage,
                                    Func<Message, bool> tryCompleteMessage,
                                    Func<Message, bool> tryDeadLetterMessage,
                                    Func<Message, bool> tryRenewMessageLock)
        {
            this.tryAbandonMessage = tryAbandonMessage;
            this.tryCompleteMessage = tryCompleteMessage;
            this.tryDeadLetterMessage = tryDeadLetterMessage;
            this.tryRenewMessageLock = tryRenewMessageLock;

            Id = sqsMessage.MessageId;
            SqsMessage = sqsMessage;
            Message = message;

            if (sqsMessage.Attributes.ContainsKey(AwsSqsMessageAttributes.ApproximateReceiveCount))
                DeliveryCount = sqsMessage.Attributes[AwsSqsMessageAttributes.ApproximateReceiveCount].TryParseInt();
        }

        public Message SqsMessage { get; }

        public int? DeliveryCount { get; }

        public string Id { get; }

        public bool IsAbandoned { get; private set; }
        public bool IsCompleted { get; private set; }
        public bool IsDeadLettered { get; private set; }

        public T Message { get; }

        public bool TryToAbandon()
        {
            if (IsAbandoned)
                return true;

            return (IsAbandoned = tryAbandonMessage(SqsMessage));
        }

        public bool TryToComplete()
        {
            if (IsCompleted)
                return true;

            return (IsCompleted = tryCompleteMessage(SqsMessage));
        }

        public bool TryToDeadLetter()
        {
            if (IsDeadLettered)
                return true;

            return (IsDeadLettered = tryDeadLetterMessage(SqsMessage));
        }

        public bool TryToRenewLock()
        {
            return tryRenewMessageLock(SqsMessage);
        }
    }
}