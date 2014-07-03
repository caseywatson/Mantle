namespace Mantle.Messaging.Interfaces
{
    public interface IMessageContext<T>
    {
        T Model { get; set; }

        bool TryToAbandon();
        bool TryToComplete();
        bool TryToDeadLetter();
    }
}