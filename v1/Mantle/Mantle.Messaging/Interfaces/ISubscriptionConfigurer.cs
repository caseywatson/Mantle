using System;
using Mantle.Interfaces;

namespace Mantle.Messaging.Interfaces
{
    public interface ISubscriptionConfigurer<T>
        where T : class
    {
        void AddConstraint(ISubscriptionConstraint<T> constraint);
        void AddConstraint(Func<IMessageContext<T>, bool> condition);
        void AddFilter(ISubscriptionFilter<T> filter);
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