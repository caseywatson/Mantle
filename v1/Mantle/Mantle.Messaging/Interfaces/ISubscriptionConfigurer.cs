using Mantle.Interfaces;

namespace Mantle.Messaging.Interfaces
{
    public interface ISubscriptionConfigurer<T>
        where T : class
    {
        void AddConstraint(ISubscriptionConstraint<T> constraint);
        void DoAutoAbandon();
        void DoAutoComplete();
        void DoAutoDeadLetter();
        void DoNotAutoAbandon();
        void DoNotAutoComplete();
        void DoNotAutoDeadLetter();
        void SetDeadLetterDeliveryLimit(int deliveryLimit);
        void UseDeadLetterStrategy(IDeadLetterStrategy<T> deadLetterStrategy);
        void UseSerializer(ISerializer<T> serializer);
    }
}