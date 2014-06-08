using System;
using Mantle.Aws;

namespace Mantle.Messaging.Aws
{
    public class SqsSubscriberEndpointManager : SqsEndpointManager, ISubscriberEndpointManager
    {
        public SqsSubscriberEndpointManager(SqsSubscriberEndpoint endpoint, IAwsConfiguration awsConfiguration)
            : base(endpoint, awsConfiguration)
        {
        }

        public void Create<T>()
        {
            throw new NotImplementedException();
        }
    }
}