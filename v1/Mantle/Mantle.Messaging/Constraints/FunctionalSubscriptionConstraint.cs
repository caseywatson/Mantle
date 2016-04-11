using System;
using Mantle.Extensions;
using Mantle.Messaging.Interfaces;

namespace Mantle.Messaging.Constraints
{
    public class FunctionalSubscriptionConstraint<T> : ISubscriptionConstraint<T>
        where T : class
    {
        private readonly Func<IMessageContext<T>, bool> condition;

        public FunctionalSubscriptionConstraint(Func<IMessageContext<T>, bool> condition)
        {
            condition.Require(nameof(condition));
            this.condition = condition;
        }

        public bool Match(IMessageContext<T> messageContext)
        {
            messageContext.Require(nameof(messageContext));
            return condition(messageContext);
        }
    }
}