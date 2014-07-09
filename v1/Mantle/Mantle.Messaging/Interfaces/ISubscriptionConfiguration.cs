using System.Collections.Generic;
using Mantle.Interfaces;

namespace Mantle.Messaging.Interfaces
{
    public interface ISubscriptionConfiguration<T>
        where T : class
    {
        bool AutoAbandon { get; set; }
        bool AutoComplete { get; set; }
        bool AutoDeadLetter { get; set; }
        IList<ISubscriptionConstraint<T>> Constraints { get; set; }
        int DeadLetterDeliveryLimit { get; set; }
        IDeadLetterStrategy<T> DeadLetterStrategy { get; set; }
        ISerializer<T> Serializer { get; set; }
        ISubscriber<T> Subscriber { get; set; }
    }
}