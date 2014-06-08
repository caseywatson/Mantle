using System;
using Mantle.Configuration;

namespace Mantle.Messaging.Msmq
{
    public abstract class MsmqEndpoint : Endpoint, IConfigurable
    {
        public string QueuePath { get; set; }

        public void Configure(IConfigurationMetadata metadata)
        {
            if (metadata == null)
                throw new ArgumentNullException("metadata");

            Name = metadata.Name;

            if (metadata.Properties.ContainsKey(ConfigurationProperties.QueuePath))
                QueuePath = metadata.Properties[ConfigurationProperties.QueuePath];

            Validate();
        }

        public void Configure(string name, string queuePath)
        {
            Name = name;
            QueuePath = queuePath;

            Validate();
        }

        public override void Validate()
        {
            base.Validate();

            if (String.IsNullOrEmpty(QueuePath))
                throw new MessagingException("MSMQ queue path is required.");
        }

        public static class ConfigurationProperties
        {
            public const string QueuePath = "QueuePath";
        }
    }
}