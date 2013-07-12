using System;
using Mantle.Configuration;
using Mantle.Messaging;

namespace Mantle.Logging.MessagingAdapter
{
    public class MessagingLogAdapter : IConfigurable, ILogAdapter
    {
        private readonly IPublisherEndpointDirectory publisherDirectory;

        private IPublisherClient publisherClient;

        public MessagingLogAdapter(IPublisherEndpointDirectory publisherDirectory)
        {
            if (publisherDirectory == null)
                throw new ArgumentNullException("publisherDirectory");

            this.publisherDirectory = publisherDirectory;
        }

        public void Configure(IConfigurationMetadata metadata)
        {
            if (metadata == null)
                throw new ArgumentNullException("metadata");

            if (metadata.Properties.ContainsKey(ConfigurationProperties.LogEndpointName))
                Configure(metadata.Properties[ConfigurationProperties.LogEndpointName]);
        }

        public void Record(Event evt)
        {
            if (evt == null)
                throw new ArgumentNullException("evt");

            publisherClient.Publish(evt);
        }

        public void Configure(string logEndpointName)
        {
            if (String.IsNullOrEmpty(logEndpointName))
                throw new ArgumentException("Log endpoint name is required.", "logEndpointName");

            if (publisherDirectory.ContainsKey(logEndpointName) == false)
                throw new ArgumentException(
                    String.Format("The specified endpoint [{0}] was not found.", logEndpointName), "logEndpointName");

            publisherClient = publisherDirectory[logEndpointName].GetClient();
        }

        public static class ConfigurationProperties
        {
            public const string LogEndpointName = "LogEndpointName";
        }
    }
}