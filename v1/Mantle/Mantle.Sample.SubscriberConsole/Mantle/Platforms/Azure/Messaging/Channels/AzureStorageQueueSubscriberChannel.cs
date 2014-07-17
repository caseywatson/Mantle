using System;
using Mantle.Configuration.Attributes;
using Mantle.Interfaces;
using Mantle.Messaging.Interfaces;
using Mantle.Sample.SubscriberConsole.Mantle.Platforms.Azure.Messaging.Contexts;

namespace Mantle.Sample.SubscriberConsole.Mantle.Platforms.Azure.Messaging.Channels
{
    public class AzureStorageQueueSubscriberChannel<T> : BaseAzureStorageQueueChannel<T>, ISubscriberChannel<T>
        where T : class
    {
        public AzureStorageQueueSubscriberChannel(ISerializer<T> serializer)
            : base(serializer)
        {
        }

        [Configurable]
        public override bool AutoSetup { get; set; }

        [Configurable(IsRequired = true)]
        public override string QueueName { get; set; }

        [Configurable(IsRequired = true)]
        public override string StorageConnectionString { get; set; }

        public IMessageContext<T> Receive(TimeSpan? timeout = null)
        {
            var message = ((timeout.HasValue)
                ? (CloudQueue.GetMessage(timeout))
                : (CloudQueue.GetMessage()));

            if (message == null)
                return null;

            return new AzureCloudQueueMessageContext<T>(CloudQueue, message, Serializer.Deserialize(message.AsString));
        }
    }
}