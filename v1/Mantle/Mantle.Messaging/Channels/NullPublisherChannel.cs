using Mantle.Messaging.Interfaces;

namespace Mantle.Messaging.Channels
{
    public class NullPublisherChannel<T> : IPublisherChannel<T>
    {
        public void Publish(T message)
        {
            // Poof!
        }
    }
}