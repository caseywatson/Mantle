using System;

using Mantle.WindowsServiceBus;

using Microsoft.ServiceBus.Messaging;

namespace Mantle.Messaging.WindowsServiceBus
{
    public abstract class WindowsServiceBusQueueClient : WindowsServiceBusClient
    {
        protected readonly QueueClient QueueClient;

        protected WindowsServiceBusQueueClient(WindowsServiceBusQueueEndpoint endpoint,
                                             IWindowsServiceBusConfiguration sbConfiguration)
            : base(sbConfiguration)
        {
            if (endpoint == null)
                throw new ArgumentNullException("endpoint");

            endpoint.Validate();

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
                        "An error occurred while attempting to access the specified Windows service bus queue [{0}]. See inner exception for more details.",
                        endpoint.QueueName),
                    ex);
            }
        }
    }
}