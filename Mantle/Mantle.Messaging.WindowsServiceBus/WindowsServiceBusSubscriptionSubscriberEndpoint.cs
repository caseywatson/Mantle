using System;
using Mantle.Configuration;
using Mantle.WindowsServiceBus;

namespace Mantle.Messaging.WindowsServiceBus
{
    public class WindowsServiceBusSubscriptionSubscriberEndpoint : Endpoint, ISubscriberEndpoint, IConfigurable
    {
        private readonly IWindowsServiceBusConfiguration sbConfiguration;

        public WindowsServiceBusSubscriptionSubscriberEndpoint(IWindowsServiceBusConfiguration sbConfiguration)
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
            return new WindowsServiceBusSubscriptionSubscriberClient(this, sbConfiguration);
        }

        public override void Validate()
        {
            base.Validate();

            if (String.IsNullOrEmpty(SubscriptionName))
                throw new MessagingException("Windows service bus subscription name is required.");

            if (String.IsNullOrEmpty(TopicName))
                throw new MessagingException("Windows service bus topic name is required.");
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