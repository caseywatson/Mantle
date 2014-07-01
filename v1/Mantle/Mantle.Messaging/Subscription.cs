using System;
using Mantle.Messaging.Interfaces;

namespace Mantle.Messaging
{
    public class Subscription<T>
    {
        public Func<T, bool> Condition { get; set; }
        public ISubscriber<T> Subscriber { get; set; }
    }
}