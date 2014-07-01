using System;
using System.Collections.Generic;
using System.Linq;
using Mantle.Configuration.Attributes;
using Mantle.Interfaces;
using Mantle.Messaging;
using Mantle.Messaging.Interfaces;

namespace Mantle.Hosting.Workers
{
    public class SubscriberWorker : BaseWorker
    {
        private readonly IDependencyResolver dependencyResolver;
        private readonly Dictionary<Type, List<Func<Message, bool>>> messageHandlers;
        private readonly Dictionary<string, Type> messageTypeTokens;
        private readonly List<ITypeTokenProvider> typeTokenProviders;

        public SubscriberWorker(IDependencyResolver dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver;
            messageHandlers = new Dictionary<Type, List<Func<Message, bool>>>();
            messageTypeTokens = new Dictionary<string, Type>();
            typeTokenProviders = dependencyResolver.GetAll<ITypeTokenProvider>().ToList();
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

        public virtual void SubscribeTo<T>(ISubscriptionConstraint<T> constraint = null, string subscriberName = null)
        {
            SetupMessageType<T>();

            var subscription = new Subscription<T>();

            subscription.Subscriber = ((subscriberName == null)
                ? (dependencyResolver.Get<ISubscriber<T>>())
                : (dependencyResolver.Get<ISubscriber<T>>(subscriberName)));

            messageHandlers[typeof (T)].Add(m => HandleMessage(m, subscription));
        }

        private void SetupMessageType<T>()
        {
            var tType = typeof (T);

            foreach (var typeTokenProvider in typeTokenProviders)
            {
                var typeToken = typeTokenProvider.GetTypeToken<T>();

                if (typeToken != null)
                    messageTypeTokens[typeToken] = tType;
            }

            if (messageHandlers.ContainsKey(tType) == false)
                messageHandlers.Add(tType, new List<Func<Message, bool>>());
        }

        private bool HandleMessage<T>(Message message, Subscription<T> subscription)
        {
            return true;
        }
    }
}