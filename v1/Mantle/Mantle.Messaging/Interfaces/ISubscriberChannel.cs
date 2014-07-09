using System;

namespace Mantle.Messaging.Interfaces
{
    public interface ISubscriberChannel<T>
        where T : class
    {
        IMessageContext<T> Receive(TimeSpan? timeout = null);
    }
}