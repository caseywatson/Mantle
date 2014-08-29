using System.Collections.Generic;
using System.Configuration;
using Mantle.Configuration.Attributes;
using Mantle.Interfaces;
using Mantle.Messaging.Interfaces;

namespace Mantle.Messaging.Configuration
{
    public class DefaultSubscriptionConfiguration : ISubscriptionConfiguration
    {
        [Configurable]
        public bool AutoAbandon { get; set; }

        [Configurable]
        public bool AutoComplete { get; set; }

        [Configurable]
        public bool AutoDeadLetter { get; set; }

        [Configurable]
        public int? DeadLetterDeliveryLimit { get; set; }
    }

    public class DefaultSubscriptionConfiguration<T> : ISubscriptionConfiguration<T>
        where T : class
    {
        public DefaultSubscriptionConfiguration()
        {
            Constraints = new List<ISubscriptionConstraint<T>>();
            Filters = new List<ISubscriptionFilter<T>>();
        }

        [Configurable]
        public bool AutoAbandon { get; set; }

        [Configurable]
        public bool AutoComplete { get; set; }

        [Configurable]
        public bool AutoDeadLetter { get; set; }

        public IList<ISubscriptionConstraint<T>> Constraints { get; set; }

        public IList<ISubscriptionFilter<T>> Filters { get; set; }

        [Configurable]
        public int? DeadLetterDeliveryLimit { get; set; }

        public IDeadLetterStrategy<T> DeadLetterStrategy { get; set; }

        public ISerializer<T> Serializer { get; set; }

        public ISubscriber<T> Subscriber { get; set; }

        public void Validate()
        {
            if (DeadLetterStrategy == null)
                throw new ConfigurationErrorsException("Dead letter strategy [DeadLetterStrategy] not defined.");

            if (Serializer == null)
                throw new ConfigurationErrorsException("Serializer [Serializer] not defined.");

            if (Subscriber == null)
                throw new ConfigurationErrorsException("Subscriber [Subscriber] not defined.");
        }
    }
}