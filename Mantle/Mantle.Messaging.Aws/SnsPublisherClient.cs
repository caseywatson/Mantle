using System;
using Amazon.SimpleNotificationService.Model;
using Mantle.Aws;

namespace Mantle.Messaging.Aws
{
    public class SnsPublisherClient : SnsClient, IPublisherClient
    {
        private readonly SnsPublisherEndpoint endpoint;

        public SnsPublisherClient(SnsPublisherEndpoint endpoint, IAwsConfiguration awsConfiguration)
            : base(awsConfiguration)
        {
            if (awsConfiguration == null)
                throw new ArgumentNullException("awsConfiguration");

            endpoint.Validate();

            this.endpoint = endpoint;
        }

        public void Publish<T>(T message)
        {
            try
            {
                Client.Publish(new PublishRequest(endpoint.TopicArn, message.SerializeToString()));
            }
            catch (Exception ex)
            {
                throw new MessagingException("Unable to send message. See inner exception for more details.", ex);
            }
        }
    }
}