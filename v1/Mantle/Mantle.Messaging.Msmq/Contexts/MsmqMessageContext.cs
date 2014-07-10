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
                    return true;
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
                    return true;
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
    }
}