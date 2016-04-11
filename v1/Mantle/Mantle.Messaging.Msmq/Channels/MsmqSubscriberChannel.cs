using Mantle.Configuration.Attributes;
using Mantle.Extensions;
using Mantle.Interfaces;
using Mantle.Messaging.Interfaces;
using Mantle.Messaging.Msmq.Contexts;
using System;
using System.IO;
using System.Messaging;
using System.Text;

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
            if (MessageQueue.Transactional)
                return ReceiveTransactionally(timeout);

                var message = ((timeout == null)
                    ? (MessageQueue.Receive())
                    : (MessageQueue.Receive(timeout.Value)));

                if (message == null)
                    return null;

            return new MsmqMessageContext<T>(GetBody(message), message);
        }

        public System.Threading.Tasks.Task<IMessageContext<T>> ReceiveAsync()
        {
            throw new NotImplementedException();
        }

        private IMessageContext<T> ReceiveTransactionally(TimeSpan? timeout = null)
        {
            var transaction = new MessageQueueTransaction();

            transaction.Begin();

            try
            {
                var message = ((timeout == null)
                    ? (MessageQueue.Receive(transaction))
                    : (MessageQueue.Receive(timeout.Value, transaction)));

                if (message == null)
                    return null;

                return new MsmqMessageContext<T>(GetBody(message), message, transaction);
            }
            catch
            {
                transaction.Abort();
                throw;
            }
        }

        private T GetBody(Message message)
        {
            message.BodyStream.TryToRewind();

            try
            {
                using (var streamReader = new StreamReader(message.BodyStream, Encoding.UTF8))
                    return Serializer.Deserialize(streamReader.ReadToEnd());
            }
            catch
            {
                return null;
            }
        }
    }
}