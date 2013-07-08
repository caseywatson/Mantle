using System;

namespace Mantle.Messaging.Aws
{
    public class SqsMessage<T> : Message<T>
    {
        private readonly SqsSubscriberClient client;
        private readonly string receiptHandle;

        public SqsMessage(T payload, SqsSubscriberClient client, string receiptHandle)
            : base(payload)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            if (String.IsNullOrEmpty(receiptHandle))
                throw new ArgumentException("Receipt handle is required.", "receiptHandle");

            this.client = client;
            this.receiptHandle = receiptHandle;

            CanBeAbandoned = false;
            CanBeCompleted = true;
            CanBeKilled = false;
            CanGetDeliveryCount = false;
        }

        public override void Abandon()
        {
            throw new NotSupportedException(
                "Unable to abandon Amazon SQS message. SQS messages are automatically abandoned if not completed.");
        }

        public override void Complete()
        {
            client.Delete(receiptHandle);
        }

        public override void Kill()
        {
            throw new NotSupportedException("Unable to kill Amazon SQS message.");
        }

        public override int GetDeliveryCount()
        {
            throw new NotSupportedException("Unable to get Amazon SQS message delivery count.");
        }
    }
}