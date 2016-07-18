using Mantle.Configuration.Attributes;
using Mantle.Extensions;
using Mantle.FaultTolerance.Interfaces;
using Mantle.Interfaces;
using Mantle.Messaging.Interfaces;
using Microsoft.WindowsAzure.Storage.Queue;

namespace Mantle.Messaging.Azure.Channels
{
    public class AzureStorageQueuePublisherChannel<T> : BaseAzureStorageQueueChannel<T>, IPublisherChannel<T>
        where T : class
    {
        private readonly ITransientFaultStrategy transientFaultStrategy;

        public AzureStorageQueuePublisherChannel(ISerializer<T> serializer,
                                                 ITransientFaultStrategy transientFaultStrategy)
            : base(serializer, transientFaultStrategy)
        {
            this.transientFaultStrategy = transientFaultStrategy;
        }

        [Configurable]
        public override bool AutoSetup { get; set; }

        [Configurable(IsRequired = true)]
        public override string QueueName { get; set; }

        [Configurable(IsRequired = true)]
        public override string StorageConnectionString { get; set; }

        public void Publish(T message)
        {
            message.Require(nameof(message));

            transientFaultStrategy.Try(() => CloudQueue.AddMessage(new CloudQueueMessage(Serializer.Serialize(message))));
        }
    }
}