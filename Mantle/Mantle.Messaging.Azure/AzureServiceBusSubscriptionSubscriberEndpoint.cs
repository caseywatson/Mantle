using System;
using Mantle.Azure;

namespace Mantle.Messaging.Azure
{
    public class AzureServiceSubscriptionSubscriberEndpoint : Endpoint, ISubscriberEndpoint
    {
        private readonly IAzureServiceBusConfiguration sbConfiguration;

        public AzureServiceSubscriptionSubscriberEndpoint(IAzureServiceBusConfiguration sbConfiguration)
        {
            if (sbConfiguration == null)
                throw new ArgumentNullException("sbConfiguration");

            sbConfiguration.Validate();

            this.sbConfiguration = sbConfiguration;
        }

        public string SubscriptionName { get; set; }
        public string TopicName { get; set; }

        public ISubscriberClient GetClient()
        {
            return new AzureServiceBusSubscriptionSubscriberClient(this, sbConfiguration);
        }

        public override void Validate()
        {
            base.Validate();

            if (String.IsNullOrEmpty(SubscriptionName))
                throw new MessagingException("Azure service bus subscription name is required.");

            if (String.IsNullOrEmpty(TopicName))
                throw new MessagingException("Azure service bus topic name is required.");
        }

        public void Configure(string name, string topicName, string subscriptionName)
        {
            Name = name;
            TopicName = topicName;
            SubscriptionName = subscriptionName;

            Validate();
        }
    }
}