using System.Messaging;
using Mantle.Extensions;
using Mantle.Messaging.Interfaces;

namespace Mantle.Messaging.Msmq.Contexts
{
    public class MsmqMessageContext<T> : IMessageContext<T>
        where T : class
    {
        public MsmqMessageContext(T message, Message msmqMessage, MessageQueueTransaction msmqTransaction)
        {
            message.Require("message");
            msmqMessage.Require("msmqMessage");
            msmqTransaction.Require("msmqTransaction");

            Message = message;
            MsmqMessage = msmqMessage;
            MsmqTransaction = msmqTransaction;
        }

        public int? DeliveryCount
        {
            get { return null; }
        }

        public bool IsAbandoned { get; private set; }
        public bool IsCompleted { get; private set; }
        public bool IsDeadLettered { get; private set; }

        public T Message { get; private set; }
        public Message MsmqMessage { get; private set; }
        public MessageQueueTransaction MsmqTransaction { get; private set; }

        public bool TryToAbandon()
        {
            if (MsmqTransaction != null)
            {
                try
                {
                    MsmqTransaction.Abort();
                    return (IsAbandoned = true);
                }
                finally
                {
                    MsmqTransaction.Dispose();
                }
            }

            return false;
        }

        public bool TryToComplete()
        {
            if (MsmqTransaction != null)
            {
                try
                {
                    MsmqTransaction.Commit();
                    return (IsCompleted = true);
                }
                finally
                {
                    MsmqTransaction.Dispose();
                }
            }

            return false;
        }

        public bool TryToDeadLetter()
        {
            return false;
        }

        public bool TryToRenewLock()
        {
            return false;
        }
    }
}