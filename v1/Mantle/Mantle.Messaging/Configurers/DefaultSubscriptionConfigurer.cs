using Mantle.Extensions;
using Mantle.Interfaces;
using Mantle.Messaging.Interfaces;

namespace Mantle.Messaging.Configurers
{
    public class SubscriptionConfigurer<T> : ISubscriptionConfigurer<T>
        where T : class
    {
        private readonly IDependencyResolver dependencyResolver;
        private readonly ISubscriptionConfiguration<T> templateConfiguration;

        public SubscriptionConfigurer(IDependencyResolver dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver;
            templateConfiguration = dependencyResolver.Get<ISubscriptionConfiguration<T>>();
        }

        public void AddConstraint(ISubscriptionConstraint<T> constraint)
        {
            constraint.Require("constraint");
            templateConfiguration.Constraints.Add(constraint);
        }

        public void Configure(ISubscriptionConfiguration<T> configuration)
        {
            configuration.Require("configuration");

            configuration.AutoAbandon = templateConfiguration.AutoAbandon;
            configuration.AutoComplete = templateConfiguration.AutoComplete;
            configuration.AutoDeadLetter = templateConfiguration.AutoDeadLetter;

            configuration.DeadLetterDeliveryLimit = templateConfiguration.DeadLetterDeliveryLimit;

            configuration.DeadLetterStrategy = (templateConfiguration.DeadLetterStrategy ??
                                                dependencyResolver.Get<IDeadLetterStrategy<T>>());

            configuration.Serializer = (templateConfiguration.Serializer ??
                                        dependencyResolver.Get<ISerializer<T>>());

            configuration.Subscriber = (templateConfiguration.Subscriber ??
                                        dependencyResolver.Get<ISubscriber<T>>());
        }

        public void DoAutoAbandon()
        {
            templateConfiguration.AutoAbandon = true;
        }

        public void DoAutoComplete()
        {
            templateConfiguration.AutoComplete = true;
        }

        public void DoAutoDeadLetter()
        {
            templateConfiguration.AutoDeadLetter = true;
        }

        public void DoNotAutoAbandon()
        {
            templateConfiguration.AutoAbandon = false;
        }

        public void DoNotAutoComplete()
        {
            templateConfiguration.AutoComplete = false;
        }

        public void DoNotAutoDeadLetter()
        {
            templateConfiguration.AutoDeadLetter = false;
        }

        public void SetDeadLetterDeliveryLimit(int deliveryLimit)
        {
            templateConfiguration.DeadLetterDeliveryLimit = deliveryLimit;
        }

        public void SetSubscriber(ISubscriber<T> subscriber)
        {
            subscriber.Require("subscriber");
            templateConfiguration.Subscriber = subscriber;
        }

        public void UseDeadLetterStrategy(IDeadLetterStrategy<T> deadLetterStrategy)
        {
            deadLetterStrategy.Require("deadLetterStrategy");
            templateConfiguration.DeadLetterStrategy = deadLetterStrategy;
        }

        public void UseSerializer(ISerializer<T> serializer)
        {
            serializer.Require("serializer");
            templateConfiguration.Serializer = serializer;
        }
    }
}