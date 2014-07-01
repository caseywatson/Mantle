using System;
using System.Collections.Generic;
using Mantle.Configuration.Attributes;
using Mantle.Interfaces;
using Mantle.Messaging;
using Mantle.Messaging.Interfaces;

namespace Mantle.Hosting.Workers
{
    public class SubscriberWorker : BaseWorker
    {
        private readonly IDependencyResolver dependencyResolver;

        public SubscriberWorker(IDependencyResolver dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver;
        }

        [Configurable]
        public bool AutoAbandon { get; set; }

        [Configurable]
        public bool AutoComplete { get; set; }

        [Configurable]
        public bool AutoDeadLetter { get; set; }

        [Configurable]
        public int DeadLetterDeliveryLimit { get; set; }

        [Configurable(IsRequired = true)]
        public string SubscriberChannelName { get; set; }

        public override void Start()
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }
    }
}