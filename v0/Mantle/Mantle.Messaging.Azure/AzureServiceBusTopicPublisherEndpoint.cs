using System;
using Mantle.Azure;
using Mantle.Configuration;

namespace Mantle.Messaging.Azure
{
    public class AzureServiceBusTopicPublisherEndpoint : Endpoint, IPublisherEndpoint, IConfigurable
    {
        private readonly IAzureServiceBusConfiguration sbConfiguration;

        public AzureServiceBusTopicPublisherEndpoint(IAzureServiceBusConfiguration sbConfiguration)
        {
            if (sbConfiguration == null)
                throw new ArgumentNullException("sbConfiguration");

            sbConfiguration.Validate();

            this.sbConfiguration = sbConfiguration;
        }

        public string TopicName { get; set; }

        public void Configure(IConfigurationMetadata metadata)
        {
            if (metadata == null)
                throw new ArgumentNullException("metadata");

            Name = metadata.Name;

            if (metadata.Properties.ContainsKey(ConfigurationProperties.TopicName))
                TopicName = metadata.Properties[ConfigurationProperties.TopicName];

            Validate();
        }

        public IPublisherClient GetClient()
        {
            return new AzureServiceBusTopicPublisherClient(this, sbConfiguration);
        }

        public override void Validate()
        {
            base.Validate();

            if (String.IsNullOrEmpty(TopicName))
                throw new MessagingException("Azure service bus topic name is required.");
        }

        public IPublisherEndpointManager GetManager()
        {
            return new AzureServiceBusTopicPublisherEndpointManager(this, sbConfiguration);
        }

        public void Configure(string name, string topicName)
        {
            Name = name;
            TopicName = topicName;

            Validate();
        }

        public static class ConfigurationProperties
        {
            public const string TopicName = "TopicName";
        }
    }
}