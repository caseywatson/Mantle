using System;
using Mantle.Azure;
using Microsoft.ServiceBus;

namespace Mantle.Messaging.Azure
{
    public abstract class AzureServiceBusClient
    {
        protected readonly NamespaceManager NsManager;

        protected AzureServiceBusClient(IAzureServiceBusConfiguration sbConfiguration)
        {
            if (sbConfiguration == null)
                throw new ArgumentNullException("sbConfiguration");

            sbConfiguration.Validate();

            try
            {
                NsManager = NamespaceManager.CreateFromConnectionString(sbConfiguration.ConnectionString);
            }
            catch (Exception ex)
            {
                throw new MessagingException(
                    "An error occurred while attempting to access the specified Azure service bus. See inner exception for more details.",
                    ex);
            }
        }
    }
}