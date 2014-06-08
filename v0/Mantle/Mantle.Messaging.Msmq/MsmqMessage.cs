using System.Messaging;

namespace Mantle.Messaging.Msmq
{
    public class MsmqMessage<T> : Message<T>, ICanBeAbandoned, ICanBeCompleted
    {
        private readonly MessageQueueTransaction transaction;

        public MsmqMessage(T payload, MessageQueueTransaction transaction = null)
            : base(payload)
        {
            this.transaction = transaction;
        }

        public void Abandon()
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

        public void Complete()
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
    }
}