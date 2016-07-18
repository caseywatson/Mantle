﻿using System;
using System.Threading.Tasks;
using Mantle.Configuration.Attributes;
using Mantle.FaultTolerance.Interfaces;
using Mantle.Messaging.Azure.Context;
using Mantle.Messaging.Interfaces;

namespace Mantle.Messaging.Azure.Channels
{
    public class AzureServiceBusQueueSubscriberChannel<T> : BaseAzureServiceBusQueueChannel, ISubscriberChannel<T>
        where T : class
    {
        private readonly ITransientFaultStrategy transientFaultStrategy;

        public AzureServiceBusQueueSubscriberChannel(ITransientFaultStrategy transientFaultStrategy)
            : base(transientFaultStrategy)
        {
            this.transientFaultStrategy = transientFaultStrategy;
        }

        [Configurable]
        public override bool AutoSetup { get; set; }

        [Configurable(IsRequired = true)]
        public override string QueueName { get; set; }

        [Configurable(IsRequired = true)]
        public override string ServiceBusConnectionString { get; set; }

        public IMessageContext<T> Receive(TimeSpan? timeout = null)
        {
            var message = ((timeout.HasValue)
                ? (transientFaultStrategy.Try(() => QueueClient.Receive(timeout.Value)))
                : (transientFaultStrategy.Try(() => QueueClient.Receive())));

            if (message == null)
                return null;

            return new AzureBrokeredMessageContext<T>(message, message.GetBody<T>());
        }

        public async Task<IMessageContext<T>> ReceiveAsync()
        {
            var message = await transientFaultStrategy.Try(() => QueueClient.ReceiveAsync());

            if (message == null)
                return null;

            return new AzureBrokeredMessageContext<T>(message, message.GetBody<T>());
        }
    }
}