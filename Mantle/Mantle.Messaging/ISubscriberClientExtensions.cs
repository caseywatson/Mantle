using System;

namespace Mantle.Messaging
{
    public static class ISubscriberClientExtensions
    {
        public static MessageScope<T> ReceiveInScope<T>(this ISubscriberClient client)
        {
            return client.ReceiveInScope<T>(TimeSpan.Zero);
        }

        public static MessageScope<T> ReceiveInScope<T>(this ISubscriberClient client, TimeSpan timeout)
        {
            if (client == null)
                throw new ArgumentNullException("client");

            return new MessageScope<T>(client.Receive<T>(timeout));
        }
    }
}