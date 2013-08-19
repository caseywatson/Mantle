using Mantle.Aws;

namespace Mantle.Messaging.Aws
{
    public class SqsPublisherEndpointManager : SqsEndpointManager, IPublisherEndpointManager
    {
        public SqsPublisherEndpointManager(SqsPublisherEndpoint endpoint, IAwsConfiguration awsConfiguration)
            : base(endpoint, awsConfiguration)
        {
        }
    }
}