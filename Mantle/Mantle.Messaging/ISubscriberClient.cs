using System;

namespace Mantle.Messaging
{
    public interface ISubscriberClient
    {
        Message<T> Receive<T>();
        Message<T> Receive<T>(TimeSpan timeout);
    }
}