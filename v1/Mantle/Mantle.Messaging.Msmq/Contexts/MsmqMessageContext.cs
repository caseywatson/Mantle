using System;
using System.Messaging;
using Mantle.Extensions;
using Mantle.Messaging.Interfaces;

namespace Mantle.Messaging.Msmq.Contexts
{
    public class MsmqMessageContext<T> : IMessageContext<T>, IDisposable
        where T : class
    {
        public MsmqMessageContext(T message, Message msmqMessage, MessageQueueTransaction msmqTransaction = null)
        {
            message.Require(nameof(message));
            msmqMessage.Require(nameof(msmqMessage));

            Message = message;
            MsmqMessage = msmqMessage;
            MsmqTransaction = msmqTransaction;
        }

        public Message MsmqMessage { get; private set; }
        public MessageQueueTransaction MsmqTransaction { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public int? DeliveryCount { get; } = null;

        public bool IsAbandoned { get; private set; }
        public bool IsCompleted { get; private set; }
        public bool IsDeadLettered { get; private set; }

        public T Message { get; }

        public bool TryToAbandon()
        {
            if ((IsAbandoned == false) && (MsmqTransaction != null))
            {
                try
                {
                    MsmqTransaction.Abort();

                    return (IsAbandoned = true);
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        public bool TryToComplete()
        {
            if ((IsCompleted == false) && (MsmqTransaction != null))
            {
                try
                {
                    MsmqTransaction.Commit();

                    return (IsCompleted = true);
                }
                catch
                {
                    return false;
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

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && (MsmqTransaction != null))
                MsmqTransaction.Dispose();
        }

        ~MsmqMessageContext()
        {
            Dispose(false);
        }
    }
}