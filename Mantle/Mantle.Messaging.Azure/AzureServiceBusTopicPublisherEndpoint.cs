using System;
using Mantle.Azure;

namespace Mantle.Messaging.Azure
{
    public class AzureServiceBusTopicPublisherEndpoint : Endpoint, IPublisherEndpoint
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

        public void Setup(string name, string topicName)
        {
            Name = name;
            TopicName = topicName;

            Validate();
        }
    }
}