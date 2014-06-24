namespace Mantle.Messaging.Interfaces
{
    public interface IMessageDistributor<T>
    {
        void Distribute(Message message);
    }
}