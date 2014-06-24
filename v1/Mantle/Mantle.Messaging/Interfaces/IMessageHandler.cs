namespace Mantle.Messaging.Interfaces
{
    public interface IMessageHandler<T>
    {
        bool Handle(T message);
    }
}