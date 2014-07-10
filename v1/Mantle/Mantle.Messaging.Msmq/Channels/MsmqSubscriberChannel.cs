using System;
using System.IO;
using System.Messaging;
using System.Text;
using Mantle.Configuration.Attributes;
using Mantle.Extensions;
using Mantle.Interfaces;
using Mantle.Messaging.Interfaces;

namespace Mantle.Messaging.Msmq.Channels
{
    public class MsmqSubscriberChannel<T> : BaseMsmqChannel<T>, ISubscriberChannel<T>
        where T : class
    {
        public MsmqSubscriberChannel(ISerializer<T> serializer)
            : base(serializer)
        {
        }

        [Configurable]
        public override bool AutoSetup { get; set; }

        [Configurable(IsRequired = true)]
        public override string QueuePath { get; set; }

        public IMessageContext<T> Receive(TimeSpan? timeout = null)
        {
            var transaction = new MessageQueueTransaction();

            transaction.Begin();

            try
            {
                var msmqMessage = ((timeout == null)
                    ? (MessageQueue.Receive(transaction))
                    : (MessageQueue.Receive(timeout.Value, transaction)));

                if (msmqMessage == null)
                    return null;

                msmqMessage.BodyStream.TryToRewind();

                T body;

                try
                {
                    using (var streamReader = new StreamReader(msmqMessage.BodyStream, Encoding.UTF8))
                        body = Serializer.Deserialize(streamReader.ReadToEnd());
                }
                catch
                {
                    body = null;
                }


            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}