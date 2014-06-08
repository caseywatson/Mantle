using System;
using Amazon;
using Amazon.SimpleNotificationService;
using Mantle.Aws;

namespace Mantle.Messaging.Aws
{
    public abstract class SnsClient
    {
        protected readonly IAmazonSimpleNotificationService Client;

        protected SnsClient(IAwsConfiguration awsConfiguration)
        {
            if (awsConfiguration == null)
                throw new ArgumentNullException("awsConfiguration");

            awsConfiguration.Validate();

            try
            {
                Client = AWSClientFactory.CreateAmazonSimpleNotificationServiceClient(awsConfiguration.AccessKey,
                                                                                      awsConfiguration.SecretKey);
            }
            catch (Exception ex)
            {
                throw new MessagingException(
                    "An error occurred while attempting to access Amazon SNS. See inner exception for more details.",
                    ex);
            }
        }
    }
}