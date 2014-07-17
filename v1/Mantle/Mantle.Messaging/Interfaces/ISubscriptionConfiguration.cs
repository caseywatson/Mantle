using System.Collections.Generic;
using Mantle.Interfaces;

namespace Mantle.Messaging.Interfaces
{
    public interface ISubscriptionConfiguration
    {
        bool AutoAbandon { get; set; }
        bool AutoComplete { get; set; }
        bool AutoDeadLetter { get; set; }
        int? DeadLetterDeliveryLimit { get; set; }
    }

    public interface ISubscriptionConfiguration<T> : ISubscriptionConfiguration
        where T : class
    {
        IList<ISubscriptionConstraint<T>> Constraints { get; set; }
        IDeadLetterStrategy<T> DeadLetterStrategy { get; set; }
        ISerializer<T> Serializer { get; set; }
        ISubscriber<T> Subscriber { get; set; }

        void Validate();
    }
}