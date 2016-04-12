using Amazon.SQS.Model;
using Mantle.Aws.Interfaces;
using Mantle.Configuration.Attributes;
using Mantle.Interfaces;
using Mantle.Messaging.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantle.Messaging.Aws.Channels
{
    public class AwsSqsSubscriberChannel<T> : BaseAwsSqsChannel<T>, ISubscriberChannel<T>
        where T : class
    {
        public AwsSqsSubscriberChannel(IAwsRegionEndpoints awsRegionEndpoints, ISerializer<T> serializer)
            : base(awsRegionEndpoints, serializer)
        { }

        [Configurable]
        public override bool AutoSetup { get; set; }

        [Configurable(IsRequired = true)]
        public override string AwsAccessKeyId { get; set; }

        [Configurable(IsRequired = true)]
        public override string AwsRegionName { get; set; }

        [Configurable(IsRequired = true)]
        public override string AwsSecretAccessKey { get; set; }

        [Configurable(IsRequired = true)]
        public override string QueueName { get; set; }

        public IMessageContext<T> Receive(TimeSpan? timeout = null)
        {
            timeout = (timeout ?? TimeSpan.FromSeconds(30));

            var receiveMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = QueueUrl,
                WaitTimeSeconds = timeout.Value.Seconds
            };

            var receiveMessageResponse = AmazonSqsClient.ReceiveMessage(receiveMessageRequest);
            var message = receiveMessageResponse.Messages?.FirstOrDefault();

            if (message == null)
                return null;

            return null; //For now.
        }

        public Task<IMessageContext<T>> ReceiveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
