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
    }
}