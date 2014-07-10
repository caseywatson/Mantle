using System.Messaging;
using Mantle.Interfaces;

namespace Mantle.Messaging.Msmq.Channels
{
    public abstract class BaseMsmqChannel<T>
        where T : class
    {
        protected readonly ISerializer<T> Serializer;

        private MessageQueue messageQueue;

        protected BaseMsmqChannel(ISerializer<T> serializer)
        {
            Serializer = serializer;
        }

        public abstract bool AutoSetup { get; set; }
        public abstract string QueuePath { get; set; }

        public MessageQueue MessageQueue
        {
            get { return GetMessageQueue(); }
        }

        private MessageQueue GetMessageQueue()
        {
            if (messageQueue != null)
            {
                if (AutoSetup)
                {
                    if (MessageQueue.Exists(QueuePath) == false)
                        MessageQueue.Create(QueuePath);
                }

                messageQueue = new MessageQueue(QueuePath);
            }

            return messageQueue;
        }
    }
}