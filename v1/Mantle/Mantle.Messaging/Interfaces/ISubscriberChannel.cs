using System;

namespace Mantle.Messaging.Interfaces
{
    public interface ISubscriberChannel<T>
    {
        T Receive(TimeSpan? timeout = null);
    }
}