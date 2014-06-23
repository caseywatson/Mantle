namespace Mantle.Messaging.Interfaces
{
    public interface IMessageHandler<T>
    {
        void Handle(T message);
    }
}