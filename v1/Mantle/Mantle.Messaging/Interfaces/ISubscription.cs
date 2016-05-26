using System;
using Mantle.Messaging.Messages;

namespace Mantle.Messaging.Interfaces
{
    public interface ISubscription<T>
        where T : class
    {
        event Action<string> ErrorOccurred;
        event Action<string> MessageOccurred;

        ISubscriptionConfiguration<T> Configuration { get; }

        bool HandleMessage(IMessageContext<MessageEnvelope> messageContext);
    }
}