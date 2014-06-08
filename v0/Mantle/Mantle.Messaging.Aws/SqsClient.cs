using System;
using Amazon;
using Amazon.SQS;
using Mantle.Aws;

namespace Mantle.Messaging.Aws
{
    public abstract class SqsClient
    {
        protected readonly IAmazonSQS Client;

        protected SqsClient(IAwsConfiguration awsConfiguration)
        {
            if (awsConfiguration == null)
                throw new ArgumentNullException("awsConfiguration");

            awsConfiguration.Validate();

            try
            {
                Client = AWSClientFactory.CreateAmazonSQSClient(awsConfiguration.AccessKey, awsConfiguration.SecretKey);
            }
            catch (Exception ex)
            {
                throw new MessagingException(
                    "An error occurred while attempting to access SQS. See inner exception for more details.",
                    ex);
            }
        }
    }
}