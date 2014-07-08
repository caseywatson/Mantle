using System;

namespace Mantle.Messaging.Interfaces
{
    public interface ISubscriberChannel<T>
    {
        IMessageContext<T> Receive(TimeSpan? timeout = null);
    }
}