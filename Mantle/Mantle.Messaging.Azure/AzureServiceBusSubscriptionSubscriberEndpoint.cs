using System;
using Mantle.Azure;
using Mantle.Configuration;

namespace Mantle.Messaging.Azure
{
    public class AzureServiceBusSubscriptionSubscriberEndpoint : Endpoint, ISubscriberEndpoint, IConfigurable
    {
        private readonly IAzureServiceBusConfiguration sbConfiguration;

        public AzureServiceBusSubscriptionSubscriberEndpoint(IAzureServiceBusConfiguration sbConfiguration)
        {
            if (sbConfiguration == null)
                throw new ArgumentNullException("sbConfiguration");

            sbConfiguration.Validate();

            this.sbConfiguration = sbConfiguration;
        }

        public string SubscriptionName { get; set; }
        public string TopicName { get; set; }

        public void Configure(IConfigurationMetadata metadata)
        {
            if (metadata == null)
                throw new ArgumentNullException("metadata");

            Name = metadata.Name;

            if (metadata.Properties.ContainsKey(ConfigurationProperties.TopicName))
                TopicName = metadata.Properties[ConfigurationProperties.TopicName];

            if (metadata.Properties.ContainsKey(ConfigurationProperties.SubscriptionName))
                SubscriptionName = metadata.Properties[ConfigurationProperties.SubscriptionName];

            Validate();
        }

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

        public static class ConfigurationProperties
        {
            public const string TopicName = "TopicName";
            public const string SubscriptionName = "SubscriptionName";
        }
    }
}