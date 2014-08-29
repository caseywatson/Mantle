using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Mantle.Extensions;
using Mantle.Hosting.Workers;
using Mantle.Interfaces;
using Mantle.Messaging.Configuration;
using Mantle.Messaging.Configurers;
using Mantle.Messaging.Interfaces;
using Mantle.Messaging.Messages;
using Mantle.Messaging.Subscriptions;

namespace Mantle.Hosting.Messaging.Workers
{
    public class SubscriptionWorker : BaseWorker
    {
        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly IDeadLetterStrategy<MessageEnvelope> defaultDeadLetterStrategy;
        private readonly ISubscriptionConfiguration defaultSubscriptionConfiguration;
        private readonly IDependencyResolver dependencyResolver;
        private readonly Dictionary<Type, List<Func<IMessageContext<MessageEnvelope>, bool>>> messageHandlers;
        private readonly List<ISubscriberChannel<MessageEnvelope>> subscriberChannels;
        private readonly List<ITypeTokenProvider> typeTokenProviders;
        private readonly Dictionary<string, Type> typeTokens;

        public SubscriptionWorker(IDependencyResolver dependencyResolver)
        {
            this.dependencyResolver = dependencyResolver;

            cancellationTokenSource = new CancellationTokenSource();
            defaultDeadLetterStrategy = dependencyResolver.Get<IDeadLetterStrategy<MessageEnvelope>>();
            defaultSubscriptionConfiguration = dependencyResolver.Get<ISubscriptionConfiguration>();
            messageHandlers = new Dictionary<Type, List<Func<IMessageContext<MessageEnvelope>, bool>>>();
            subscriberChannels = new List<ISubscriberChannel<MessageEnvelope>>();
            typeTokenProviders = dependencyResolver.GetAll<ITypeTokenProvider>().ToList();
            typeTokens = new Dictionary<string, Type>();
        }

        public override void Start()
        {
            if (subscriberChannels.None())
                subscriberChannels.AddRange(dependencyResolver.GetAll<ISubscriberChannel<MessageEnvelope>>());

            for (int i = 0; i < subscriberChannels.Count; i++)
            {
                var subscriberChannel = subscriberChannels[i];

                if (i == (subscriberChannels.Count - 1))
                {
                    SubscribeToChannel(subscriberChannel, cancellationTokenSource.Token);
                }
                else
                {
                    var channelThread =
                        new Thread(() => SubscribeToChannel(subscriberChannel, cancellationTokenSource.Token));

                    channelThread.Start();
                }
            }
        }

        public override void Stop()
        {
            if (cancellationTokenSource.IsCancellationRequested == false)
                cancellationTokenSource.Cancel();
        }

        public void AddSubscriberChannel(ISubscriberChannel<MessageEnvelope> subscriberChannel)
        {
            subscriberChannel.Require("subscriberChannel");
            subscriberChannels.Add(subscriberChannel);
        }

        public void SubscribeTo<T>()
            where T : class
        {
            SubscribeTo<T>(c => { });
        }

        public void SubscribeTo<T>(Action<ISubscriptionConfigurer<T>> configurerAction)
            where T : class
        {
            configurerAction.Require("configurerAction");

            var configuration = new DefaultSubscriptionConfiguration<T>
            {
                AutoAbandon = defaultSubscriptionConfiguration.AutoAbandon,
                AutoComplete = defaultSubscriptionConfiguration.AutoComplete,
                AutoDeadLetter = defaultSubscriptionConfiguration.AutoDeadLetter,
                DeadLetterDeliveryLimit = defaultSubscriptionConfiguration.DeadLetterDeliveryLimit
            };

            var configurer = new DefaultSubscriptionConfigurer<T>(configuration);

            configurerAction(configurer); 

            if (configuration.Filters.None())
                configuration.Filters = dependencyResolver.GetAll<ISubscriptionFilter<T>>().ToList();

            configuration.DeadLetterStrategy = (configuration.DeadLetterStrategy ??
                                                dependencyResolver.Get<IDeadLetterStrategy<T>>());

            configuration.Serializer = (configuration.Serializer ??
                                        dependencyResolver.Get<ISerializer<T>>());

            configuration.Subscriber = (configuration.Subscriber ??
                                        dependencyResolver.Get<ISubscriber<T>>());

            SubscribeTo(configuration);
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

        private bool HandleMessage(IMessageContext<MessageEnvelope> messageContext)
        {
            foreach (string typeToken in messageContext.Message.BodyTypeTokens)
            {
                if ((typeToken != null) && (typeTokens.ContainsKey(typeToken)))
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

        private void HandleMessageEnvelope(IMessageContext<MessageEnvelope> messageEnvelope)
        {
            if (messageEnvelope != null)
            {
                if ((messageEnvelope.Message == null) || (HandleMessage(messageEnvelope) == false))
                {
                    if ((defaultSubscriptionConfiguration.AutoAbandon &&
                         defaultSubscriptionConfiguration.DeadLetterDeliveryLimit.HasValue &&
                         messageEnvelope.DeliveryCount.HasValue) &&
                        (messageEnvelope.DeliveryCount.Value >=
                         defaultSubscriptionConfiguration.DeadLetterDeliveryLimit.Value))
                    {
                        defaultDeadLetterStrategy.HandleDeadLetterMessage(messageEnvelope);
                    }
                    else
                    {
                        messageEnvelope.TryToComplete();
                    }
                }
            }
        }

        private void SetupSubscriptionType<T>()
        {
            Type tType = (typeof (T));

            if (messageHandlers.ContainsKey(tType) == false)
            {
                foreach (ITypeTokenProvider typeTokenProvider in typeTokenProviders)
                {
                    var typeToken = typeTokenProvider.GetTypeToken<T>();

                    if (typeToken != null)
                        typeTokens.Add(typeToken, tType);
                }
                messageHandlers.Add(tType, new List<Func<IMessageContext<MessageEnvelope>, bool>>());
            }
        }

        private void SubscribeToChannel(ISubscriberChannel<MessageEnvelope> channel,
                                        CancellationToken cancellationToken)
        {
            while (true)
            {
                var messageContext = channel.Receive();

                lock (this)
                    HandleMessageEnvelope(messageContext);

                if (cancellationToken.IsCancellationRequested)
                    return;
            }
        }
    }
}