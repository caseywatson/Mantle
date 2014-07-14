using System;
using System.Collections.Generic;
using System.Linq;
using Mantle.Configuration.Attributes;
using Mantle.Extensions;
using Mantle.Hosting.Workers;
using Mantle.Interfaces;
using Mantle.Messaging.Interfaces;
using Mantle.Messaging.Messages;
using Mantle.Messaging.Subscriptions;

namespace Mantle.Hosting.Messaging.Workers
{
    public class SubscriptionWorker : BaseWorker
    {
        private readonly IDeadLetterStrategy<MessageEnvelope> defaultDeadLetterStrategy;
        private readonly IDependencyResolver dependencyResolver;
        private readonly Dictionary<Type, List<Func<IMessageContext<MessageEnvelope>, bool>>> messageHandlers;
        private readonly List<ITypeTokenProvider> typeTokenProviders;
        private readonly Dictionary<string, Type> typeTokens;

        private bool doStop;
        private ISubscriberChannel<MessageEnvelope> subscriberChannel;

        public SubscriptionWorker(IDependencyResolver dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver;

            defaultDeadLetterStrategy = dependencyResolver.Get<IDeadLetterStrategy<MessageEnvelope>>();
            messageHandlers = new Dictionary<Type, List<Func<IMessageContext<MessageEnvelope>, bool>>>();
            typeTokenProviders = dependencyResolver.GetAll<ITypeTokenProvider>().ToList();
            typeTokens = new Dictionary<string, Type>();
        }

        [Configurable]
        public bool AutoDeadLetter { get; set; }

        [Configurable]
        public int? DeadLetterDeliveryLimit { get; set; }

        public override void Start()
        {
            doStop = false;

            subscriberChannel = (subscriberChannel ??
                                 dependencyResolver.Get<ISubscriberChannel<MessageEnvelope>>());

            while (true)
            {
                if (doStop) return;

                IMessageContext<MessageEnvelope> message = subscriberChannel.Receive();

                if (message != null)
                {
                    if ((message.Message == null) || (HandleMessage(message) == false))
                    {
                        if ((AutoDeadLetter && DeadLetterDeliveryLimit.HasValue && message.DeliveryCount.HasValue) &&
                            (message.DeliveryCount.Value >= DeadLetterDeliveryLimit.Value))
                        {
                            defaultDeadLetterStrategy.HandleDeadLetterMessage(message);
                        }
                        else
                        {
                            message.TryToComplete();
                        }
                    }
                }
            }
        }

        public override void Stop()
        {
            doStop = true;
        }

        public void SubscribeTo<T>()
            where T : class
        {
            SubscribeTo<T>(c => { });
        }

        public void SubscribeTo<T>(Action<ISubscriptionConfigurationBuilder<T>> configurer)
            where T : class
        {
            configurer.Require("configurer");

            var configurationBuilder = dependencyResolver.Get<ISubscriptionConfigurationBuilder<T>>();

            configurer(configurationBuilder);

            SubscribeTo(configurationBuilder.ToConfiguration());
        }

        public void SubscribeTo<T>(ISubscriptionConfiguration<T> configuration)
            where T : class
        {
            configuration.Require("configuration");

            SubscribeTo(new DefaultSubscription<T>(configuration));
        }

        public void SubscribeTo<T>(ISubscription<T> subscription)
            where T : class
        {
            subscription.Require("subscription");

            SetupSubscriptionType<T>();
            messageHandlers[typeof (T)].Add(subscription.HandleMessage);
        }

        public void UseSubscriberChannel(ISubscriberChannel<MessageEnvelope> subscriberChannel)
        {
            subscriberChannel.Require("subscriberChannel");
            this.subscriberChannel = subscriberChannel;
        }

        private bool HandleMessage(IMessageContext<MessageEnvelope> messageContext)
        {
            foreach (string typeToken in messageContext.Message.BodyTypeTokens)
            {
                if (typeTokens.ContainsKey(typeToken))
                {
                    foreach (var handlerFunction in messageHandlers[typeTokens[typeToken]])
                    {
                        if (handlerFunction(messageContext))
                            return true;
                    }
                }
            }

            return false;
        }

        private void SetupSubscriptionType<T>()
        {
            Type tType = (typeof (T));

            if (messageHandlers.ContainsKey(tType) == false)
            {
                foreach (ITypeTokenProvider typeTokenProvider in typeTokenProviders)
                    typeTokens.Add(typeTokenProvider.GetTypeToken<T>(), tType);

                messageHandlers.Add(tType, new List<Func<IMessageContext<MessageEnvelope>, bool>>());
            }
        }
    }
}