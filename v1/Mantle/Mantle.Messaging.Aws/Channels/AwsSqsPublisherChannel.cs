using Mantle.Aws.Interfaces;
using Mantle.Configuration.Attributes;
using Mantle.Extensions;
using Mantle.Interfaces;
using Mantle.Messaging.Interfaces;

namespace Mantle.Messaging.Aws.Channels
{
    public class AwsSqsPublisherChannel<T> : BaseAwsSqsChannel<T>, IPublisherChannel<T>
        where T : class
    {
        public AwsSqsPublisherChannel(IAwsRegionEndpoints awsRegionEndpoints, ISerializer<T> serializer)
            : base(awsRegionEndpoints, serializer)
        { }

        [Configurable]
        public override bool AutoSetup { get; set; }

        [Configurable(IsRequired = true)]
        public override string AwsAccessKeyId { get; set; }

        [Configurable(IsRequired = true)]
        public override string AwsRegionName { get; set; }

        [Configurable(IsRequired = true)]
        public override string AwsSecretAccessKey { get; set; }

        [Configurable(IsRequired = true)]
        public override string QueueName { get; set; }

        public void Publish(T message)
        {
            message.Require(nameof(message));

            AwsSqsClient.SendMessage(QueueUrl, Serializer.Serialize(message));
        }
    }
}
