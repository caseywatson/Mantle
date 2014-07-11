using System;
using System.Linq;
using Amazon.SQS.Model;
using Mantle.Configuration.Attributes;
using Mantle.Interfaces;
using Mantle.Messaging.Aws.Contexts;
using Mantle.Messaging.Interfaces;

namespace Mantle.Messaging.Aws.Channels
{
    public class AwsSqsSubscriberChannel<T> : BaseAwsSqsChannel<T>, ISubscriberChannel<T>
        where T : class
    {
        public AwsSqsSubscriberChannel(ISerializer<T> serializer)
            : base(serializer)
        {
        }

        [Configurable(IsRequired = true)]
        public override string AwsAccessKey { get; set; }

        [Configurable(IsRequired = true)]
        public override string AwsSecretAccessKey { get; set; }

        [Configurable(IsRequired = true)]
        public override string QueueUrl { get; set; }

        public IMessageContext<T> Receive(TimeSpan? timeout = null)
        {
            var receiveResponse = SqsClient.ReceiveMessage(new ReceiveMessageRequest {QueueUrl = QueueUrl});
            var message = receiveResponse.Messages.FirstOrDefault();

            if (message == null)
                return null;

            return new AwsSqsMessageContext<T>(this, Serializer.Deserialize(message.Body), message.ReceiptHandle);
        }
    }
}