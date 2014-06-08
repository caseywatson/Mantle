using System;
using Amazon.SQS.Model;
using Mantle.Aws;

namespace Mantle.Messaging.Aws
{
    public class SqsPublisherClient : SqsClient, IPublisherClient
    {
        private readonly SqsPublisherEndpoint endpoint;

        public SqsPublisherClient(SqsPublisherEndpoint endpoint, IAwsConfiguration awsConfiguration)
            : base(awsConfiguration)
        {
            if (endpoint == null)
                throw new ArgumentNullException("endpoint");

            endpoint.Validate();

            this.endpoint = endpoint;
        }

        public void Publish<T>(T message)
        {
            try
            {
                Client.SendMessage(new SendMessageRequest
                    {
                        QueueUrl = endpoint.QueueUrl,
                        MessageBody = message.SerializeToString()
                    });
            }
            catch (Exception ex)
            {
                throw new MessagingException("Unable to send message. See inner exception for more details.", ex);
            }
        }
    }
}