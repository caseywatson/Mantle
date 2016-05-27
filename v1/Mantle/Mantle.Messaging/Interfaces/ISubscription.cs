using System;
using Mantle.Messaging.Messages;

namespace Mantle.Messaging.Interfaces
{
    public interface ISubscription<T>
        where T : class
    {
        ISubscriptionConfiguration<T> Configuration { get; }
        event Action<string> ErrorOccurred;
        event Action<string> MessageOccurred;

        bool HandleMessage(IMessageContext<MessageEnvelope> messageContext);
    }
}