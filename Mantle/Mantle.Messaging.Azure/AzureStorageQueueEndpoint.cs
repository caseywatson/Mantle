using System;
using Mantle.Azure;

namespace Mantle.Messaging.Azure
{
    public abstract class AzureStorageQueueEndpoint : Endpoint
    {
        protected readonly IAzureStorageConfiguration StorageConfiguration;

        protected AzureStorageQueueEndpoint(IAzureStorageConfiguration storageConfiguration)
        {
            if (storageConfiguration == null)
                throw new ArgumentNullException("storageConfiguration");

            storageConfiguration.Validate();

            StorageConfiguration = storageConfiguration;
        }

        public string QueueName { get; set; }

        public void Setup(string name, string queueName)
        {
            Name = name;
            QueueName = queueName;

            Validate();
        }

        public override void Validate()
        {
            base.Validate();

            if (String.IsNullOrEmpty(QueueName))
                throw new MessagingException("Azure service bus queue name is required.");
        }
    }
}