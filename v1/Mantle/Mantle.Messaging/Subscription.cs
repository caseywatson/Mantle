using System.Collections.Generic;
using Mantle.Messaging.Interfaces;

namespace Mantle.Messaging
{
    public class Subscription<T>
    {
        public ISubscriptionConstraint<T> Constraint { get; set; }
        public ISubscriber<T> Subscriber { get; set; }
    }
}