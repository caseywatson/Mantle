using System;

namespace Mantle.Messaging.Aws
{
    public abstract class SqsEndpoint : Endpoint
    {
        public string QueueUrl { get; set; }

        public override void Validate()
        {
            base.Validate();

            if (String.IsNullOrEmpty(QueueUrl))
                throw new MessagingException("SQS queue URL is required.");
        }
    }
}