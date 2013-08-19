using System;
using Mantle.Azure;
using Microsoft.ServiceBus.Messaging;

namespace Mantle.Messaging.Azure
{
    public abstract class AzureServiceBusQueueClient : AzureServiceBusClient
    {
        protected readonly QueueClient QueueClient;
        protected readonly AzureServiceBusQueueEndpoint QueueEndpoint;

        protected AzureServiceBusQueueClient(AzureServiceBusQueueEndpoint endpoint,
                                             IAzureServiceBusConfiguration sbConfiguration)
            : base(sbConfiguration)
        {
            if (endpoint == null)
                throw new ArgumentNullException("endpoint");

            endpoint.Validate();

            QueueEndpoint = endpoint;

            try
            {
                if (NsManager.QueueExists(endpoint.QueueName) == false)
                    NsManager.CreateQueue(endpoint.QueueName);

                QueueClient = QueueClient.CreateFromConnectionString(sbConfiguration.ConnectionString,
                                                                     endpoint.QueueName);
            }
            catch (Exception ex)
            {
                throw new MessagingException(
                    String.Format(
                        "An error occurred while attempting to access the specified Azure service bus queue [{0}]. See inner exception for more details.",
                        endpoint.QueueName),
                    ex);
            }
        }
    }
}