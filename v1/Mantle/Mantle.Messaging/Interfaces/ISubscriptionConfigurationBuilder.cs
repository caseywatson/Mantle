using Mantle.Interfaces;

namespace Mantle.Messaging.Interfaces
{
    public interface ISubscriptionConfigurationBuilder<T>
        where T : class
    {
        void AddConstraint(ISubscriptionConstraint<T> constraint);
        ISubscriptionConfiguration<T> ToConfiguration();
        void DoAutoAbandon();
        void DoAutoComplete();
        void DoAutoDeadLetter();
        void DoNotAutoAbandon();
        void DoNotAutoComplete();
        void DoNotAutoDeadLetter();
        void SetDeadLetterDeliveryLimit(int deliveryLimit);
        void SetSubscriber(ISubscriber<T> subscriber);
        void UseDeadLetterStrategy(IDeadLetterStrategy<T> deadLetterStrategy);
        void UseSerializer(ISerializer<T> serializer);
    }
}