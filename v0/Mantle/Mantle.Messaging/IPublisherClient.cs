namespace Mantle.Messaging
{
    public interface IPublisherClient
    {
        void Publish<T>(T message);
    }
}