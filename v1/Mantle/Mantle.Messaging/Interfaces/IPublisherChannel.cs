namespace Mantle.Messaging.Interfaces
{
    public interface IPublisherChannel<T>
    {
        void Publish(T message);
    }
}