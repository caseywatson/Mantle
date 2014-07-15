using System.Linq;
using Mantle.Extensions;
using Mantle.Messaging.Contexts;
using Mantle.Messaging.Interfaces;
using Mantle.Messaging.Messages;

namespace Mantle.Messaging.Subscriptions
{
    public class DefaultSubscription<T> : ISubscription
        where T : class
    {
        private readonly ISubscriptionConfiguration<T> configuration;

        public DefaultSubscription(ISubscriptionConfiguration<T> configuration)
        {
            configuration.Require("configuration");
            configuration.Validate();
            this.configuration = configuration;
        }

        public bool HandleMessage(IMessageContext<MessageEnvelope> messageContext)
        {
            messageContext.Require("messageContext");

            T body = configuration.Serializer.Deserialize(messageContext.Message.Body);
            var context = new SubscriptionMessageContext<T>(messageContext, body);

            if (configuration.Constraints.Any(c => (c.Match(context) == false)))
                return false;

            if (configuration.AutoDeadLetter)
            {
                if ((messageContext.DeliveryCount.HasValue) && (configuration.DeadLetterDeliveryLimit.HasValue) &&

                    (messageContext.DeliveryCount.Value >= configuration.DeadLetterDeliveryLimit.Value))
                {
                    configuration.DeadLetterStrategy.HandleDeadLetterMessage(context);
                    return true;
                }
            }

            try
            {
                configuration.Subscriber.HandleMessage(context);

                if (configuration.AutoComplete)
                    context.TryToComplete();
            }
            catch
            {
                if (configuration.AutoAbandon)
                    context.TryToAbandon();

                throw;
            }

            return true;
        }
    }
}