using System;
using System.Messaging;

namespace Mantle.Messaging.Msmq
{
    public class MsmqMessage<T> : Message<T>
    {
        private readonly MessageQueueTransaction transaction;

        public MsmqMessage(T payload, MessageQueueTransaction transaction = null)
            : base(payload)
        {
            this.transaction = transaction;

            CanBeAbandoned = true;
            CanBeCompleted = true;
            CanBeKilled = false;
            CanGetDeliveryCount = false;
        }

        public override void Abandon()
        {
            if (transaction != null)
            {
                try
                {
                    transaction.Abort();
                }
                finally
                {
                    transaction.Dispose();
                }
            }
        }

        public override void Complete()
        {
            if (transaction != null)
            {
                try
                {
                    transaction.Commit();
                }
                finally
                {
                    transaction.Dispose();
                }
            }
        }

        public override void Kill()
        {
            throw new NotSupportedException("Unable to kill MSMQ message.");
        }

        public override int GetDeliveryCount()
        {
            throw new NotSupportedException("Unable to get MSMQ message delivery count.");
        }
    }
}