namespace Mantle.Messaging.Interfaces
{
    public interface ISubscriber
    {
        Message Receive();
    }
}