using System;

namespace Mantle.Messaging
{
    public class MessageScope<T> : IDisposable
    {
        public MessageScope(Message<T> message)
        {
            Message = message;
        }

        public Message<T> Message { get; private set; }

        public void Dispose()
        {
            if (Message != null)
                Message.Complete();
        }
    }
}