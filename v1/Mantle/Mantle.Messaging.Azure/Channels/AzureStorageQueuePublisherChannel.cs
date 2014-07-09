using Mantle.Configuration.Attributes;
using Mantle.Extensions;
using Mantle.Interfaces;
using Mantle.Messaging.Interfaces;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Mantle.Messaging.Azure.Channels
{
    public class AzureStorageQueuePublisherChannel<T> : BaseAzureStorageQueueChannel<T>, IPublisherChannel<T>
        where T : class
    {
        public AzureStorageQueuePublisherChannel(ISerializer<T> serializer)
            : base(serializer)
        {
        }

        [Configurable]
        public override bool AutoSetup { get; set; }

        [Configurable(IsRequired = true)]
        public override string QueueName { get; set; }

        [Configurable(IsRequired = true)]
        public override string StorageConnectionString { get; set; }

        public void Publish(T message)
        {
            message.Require("message");
            CloudQueue.AddMessage(new CloudQueueMessage(Serializer.Serialize(message)));
        }
    }
}