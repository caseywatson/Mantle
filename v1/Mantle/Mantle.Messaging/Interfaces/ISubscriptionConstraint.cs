namespace Mantle.Messaging.Interfaces
{
    public interface ISubscriptionConstraint<T>
        where T : class
    {
        bool Match(IMessageContext<T> messageContext);
    }
}