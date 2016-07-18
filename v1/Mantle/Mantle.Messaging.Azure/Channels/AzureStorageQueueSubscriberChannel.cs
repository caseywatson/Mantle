using System;
using System.Threading.Tasks;
using Mantle.Configuration.Attributes;
using Mantle.FaultTolerance.Interfaces;
using Mantle.Interfaces;
using Mantle.Messaging.Azure.Context;
using Mantle.Messaging.Interfaces;

namespace Mantle.Messaging.Azure.Channels
{
    public class AzureStorageQueueSubscriberChannel<T> : BaseAzureStorageQueueChannel<T>, ISubscriberChannel<T>
        where T : class
    {
        private readonly ITransientFaultStrategy transientFaultStrategy;

        public AzureStorageQueueSubscriberChannel(ISerializer<T> serializer,
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

        public IMessageContext<T> Receive(TimeSpan? timeout = null)
        {
            var message = ((timeout.HasValue)
                ? (transientFaultStrategy.Try(() => CloudQueue.GetMessage(timeout)))
                : (transientFaultStrategy.Try(() => CloudQueue.GetMessage())));

            if (message == null)
                return null;

            return new AzureCloudQueueMessageContext<T>(CloudQueue, message, Serializer.Deserialize(message.AsString));
        }


        public async Task<IMessageContext<T>> ReceiveAsync()
        {
            var message = await transientFaultStrategy.Try(() => CloudQueue.GetMessageAsync());

            if (message == null)
                return null;

            return new AzureCloudQueueMessageContext<T>(CloudQueue, message, Serializer.Deserialize(message.AsString));
        }
    }
}