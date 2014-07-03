using Mantle.Interfaces;

namespace Mantle.Messaging.Interfaces
{
    public interface ISubscriptionConfigurer<T>
    {
        void DoAutoAbandon();
        void DoAutoComplete();
        void DoAutoDeadLetter();

        void DoNotAutoAbandon();
        void DoNotAutoComplete();
        void DoNotAutoDeadLetter();

        int SetDeadLetterDeliveryLimit(int deliveryLimit);

        void AddConstraint(ISubscriptionConstraint<T> constraint);
        void UseSerializer(ISerializer<T> serializer);
        void UseDeadLetterStrategy(IDeadLetterStrategy<T> deadLetterStrategy);
    }
}