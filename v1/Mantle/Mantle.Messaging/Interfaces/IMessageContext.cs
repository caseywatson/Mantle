namespace Mantle.Messaging.Interfaces
{
    public interface IMessageContext<T>
    {
        T Body { get; set; }
        Message OriginalMessage { get; set; }
        bool TryToAbandon();
        bool TryToComplete();
        bool TryToDeadLetter();
    }
}