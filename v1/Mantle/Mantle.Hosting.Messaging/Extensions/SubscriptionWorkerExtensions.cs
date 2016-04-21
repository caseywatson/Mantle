using System;
using Mantle.Extensions;
using Mantle.Hosting.Messaging.Workers;

namespace Mantle.Hosting.Messaging.Extensions
{
    public static class SubscriptionWorkerExtensions
    {
        public static void SubscribeTo(this SubscriptionWorker subscriptionWorker, Type messageType)
        {
            subscriptionWorker.Require("subscriptionWorker");
            messageType.Require("messageType");

            var methodInfo = (typeof(SubscriptionWorker)).GetMethod("SubscribeTo", Type.EmptyTypes);
            var genericMethodInfo = methodInfo.MakeGenericMethod(messageType);

            genericMethodInfo.Invoke(subscriptionWorker, null);
        }

        public static void SubscribeToAll(this SubscriptionWorker subscriptionWorker, params Type[] messageTypes)
        {
            subscriptionWorker.Require("subscriptionWorker");
            messageTypes.Require("messageTypes");

            foreach (var messageType in messageTypes)
                subscriptionWorker.SubscribeTo(messageType);
        }
    }
}