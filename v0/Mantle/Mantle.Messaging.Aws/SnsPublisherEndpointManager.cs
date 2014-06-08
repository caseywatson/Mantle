using System;
using System.Linq;
using System.Net;
using Amazon.SimpleNotificationService.Model;
using Mantle.Aws;

namespace Mantle.Messaging.Aws
{
    public class SnsPublisherEndpointManager : SnsClient, IPublisherEndpointManager
    {
        private readonly SnsPublisherEndpoint endpoint;

        public SnsPublisherEndpointManager(SnsPublisherEndpoint endpoint, IAwsConfiguration awsConfiguration)
            : base(awsConfiguration)
        {
            if (endpoint == null)
                throw new ArgumentNullException("endpoint");

            endpoint.Validate();

            this.endpoint = endpoint;
        }

        public bool DoesExist()
        {
            ListTopicsResponse response = Client.ListTopics(new ListTopicsRequest());

            if (response.HttpStatusCode != HttpStatusCode.OK)
                throw new MessagingException(String.Format(
                    "An error occurred while attempting to determine whether the specified topic exists. AWS returned status code [{0}].",
                    response.HttpStatusCode));

            if ((response.Topics == null) || (response.Topics.Count == 0))
                return false;

            return
                response
                    .Topics
                    .Select(t => t.TopicArn)
                    .Any(a => (String.Compare(a, endpoint.TopicArn, StringComparison.InvariantCultureIgnoreCase) == 0));
        }

        public void Create()
        {
            throw new NotImplementedException();
        }
    }
}