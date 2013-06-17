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
        }

        public override void Abandon()
        {
            // Do nothing... SQS will release the message automatically.
        }

        public override void Complete()
        {
            client.Delete(receiptHandle);
        }
    }
}