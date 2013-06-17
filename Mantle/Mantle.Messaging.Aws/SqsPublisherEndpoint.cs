using System;
using Mantle.Aws;

namespace Mantle.Messaging.Aws
{
    public class SqsPublisherEndpoint : SqsEndpoint, IPublisherEndpoint
    {
        private readonly IAwsConfiguration awsConfiguration;

        public SqsPublisherEndpoint(AwsConfiguration awsConfiguration)
        {
            if (awsConfiguration == null)
                throw new ArgumentNullException("awsConfiguration");

            awsConfiguration.Validate();

            this.awsConfiguration = awsConfiguration;
        }

        public IPublisherClient GetClient()
        {
            return new SqsPublisherClient(this, awsConfiguration);
        }
    }
}