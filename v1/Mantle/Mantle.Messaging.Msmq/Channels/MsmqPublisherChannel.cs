using System.IO;
using System.Messaging;
using System.Text;
using Mantle.Configuration.Attributes;
using Mantle.Extensions;
using Mantle.Interfaces;
using Mantle.Messaging.Interfaces;

namespace Mantle.Messaging.Msmq.Channels
{
    public class MsmqPublisherChannel<T> : BaseMsmqChannel<T>, IPublisherChannel<T>
        where T : class
    {
        public MsmqPublisherChannel(ISerializer<T> serializer)
            : base(serializer)
        {
        }

        [Configurable]
        public override bool AutoSetup { get; set; }

        [Configurable(IsRequired = true)]
        public override string QueuePath { get; set; }

        public void Publish(T message)
        {
            message.Require("message");

            var msmqMessage = new Message
            {
                BodyStream = new MemoryStream(Encoding.UTF8.GetBytes(Serializer.Serialize(message)))
            };

            MessageQueue.Send(msmqMessage, MessageQueueTransactionType.Single);
        }
    }
}