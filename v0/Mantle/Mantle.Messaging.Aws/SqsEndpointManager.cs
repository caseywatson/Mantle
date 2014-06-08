using System;
using System.Linq;
using System.Net;
using Amazon.SQS.Model;
using Mantle.Aws;

namespace Mantle.Messaging.Aws
{
    public abstract class SqsEndpointManager : SqsClient, IEndpointManager
    {
        protected SqsEndpointManager(SqsEndpoint endpoint, IAwsConfiguration awsConfiguration)
            : base(awsConfiguration)
        {
            Endpoint = endpoint;
        }

        protected SqsEndpoint Endpoint { get; private set; }

        public bool DoesExist()
        {
            ListQueuesResponse response = Client.ListQueues(new ListQueuesRequest());

            if (response.HttpStatusCode != HttpStatusCode.OK)
                throw new MessagingException(String.Format(
                    "An error occurred while attempting to determine whether the specified queue exists. AWS returned status code [{0}].",
                    response.HttpStatusCode));

            if ((response.QueueUrls == null) || (response.QueueUrls.Count == 0))
                return false;

            return response.QueueUrls.Any(
                q => (String.Compare(q, Endpoint.QueueUrl, StringComparison.InvariantCultureIgnoreCase) == 0));
        }

        public void Create()
        {
            throw new NotImplementedException();
        }
    }
}