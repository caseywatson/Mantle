using System;
using Mantle.Azure;

namespace Mantle.Messaging.Azure
{
    public class AzureServiceBusQueuePublisherEndpoint : AzureServiceBusQueueEndpoint, IPublisherEndpoint
    {
        private readonly IAzureServiceBusConfiguration sbConfiguration;

        public AzureServiceBusQueuePublisherEndpoint(IAzureServiceBusConfiguration sbConfiguration)
        {
            if (sbConfiguration == null)
                throw new ArgumentNullException("sbConfiguration");

            sbConfiguration.Validate();

            this.sbConfiguration = sbConfiguration;
        }

        public IPublisherClient GetClient()
        {
            return new AzureServiceBusQueuePublisherClient(this, sbConfiguration);
        }
    }
}