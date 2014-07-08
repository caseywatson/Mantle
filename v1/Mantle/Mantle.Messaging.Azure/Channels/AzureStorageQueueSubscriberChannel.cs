using System;
using Mantle.Configuration.Attributes;
using Mantle.Messaging.Interfaces;

namespace Mantle.Messaging.Azure.Channels
{
    public class AzureStorageQueueSubscriberChannel<T> : ISubscriberChannel<T>
    {


        [Configurable]
        public bool AutoSetup { get; set; }

        [Configurable(IsRequired = true)]
        public string StorageConnectionString { get; set; }

        [Configurable(IsRequired = true)]
        public string QueueName { get; set; }

        public IMessageContext<T> Receive(TimeSpan? timeout = null)
        {
            
        }
    }
}