using Amazon.SQS.Model;
using Mantle.Configuration.Attributes;
using Mantle.Extensions;
using Mantle.Interfaces;
using Mantle.Messaging.Interfaces;

namespace Mantle.Messaging.Aws.Channels
{
    public class AwsSqsPublisherChannel<T> : BaseAwsSqsChannel<T>, IPublisherChannel<T>
        where T : class
    {
        public AwsSqsPublisherChannel(ISerializer<T> serializer)
            : base(serializer)
        {
        }

        [Configurable(IsRequired = true)]
        public override string AwsAccessKey { get; set; }

        [Configurable(IsRequired = true)]
        public override string AwsSecretAccessKey { get; set; }

        [Configurable(IsRequired = true)]
        public override string QueueUrl { get; set; }

        public void Publish(T message)
        {
            message.Require("message");

            SqsClient.SendMessage(new SendMessageRequest
            {
                QueueUrl = QueueUrl,
                MessageBody = Serializer.Serialize(message)
            });
        }
    }
}