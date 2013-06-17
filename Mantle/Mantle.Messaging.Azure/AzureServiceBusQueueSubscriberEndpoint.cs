using System;
using Mantle.Azure;

namespace Mantle.Messaging.Azure
{
    public class AzureServiceBusQueueSubscriberEndpoint : AzureServiceBusQueueEndpoint, ISubscriberEndpoint
    {
        private readonly IAzureServiceBusConfiguration sbConfiguration;

        public AzureServiceBusQueueSubscriberEndpoint(IAzureServiceBusConfiguration sbConfiguration)
        {
            if (sbConfiguration == null)
                throw new ArgumentNullException("sbConfiguration");

            sbConfiguration.Validate();

            this.sbConfiguration = sbConfiguration;
        }

        public ISubscriberClient GetClient()
        {
            return new AzureServiceBusQueueSubscriberClient(this, sbConfiguration);
        }
    }
}