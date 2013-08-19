using System;

namespace Mantle.Messaging.Msmq
{
    public class MsmqSubscriberEndpointManager : MsmqEndpointManager, ISubscriberEndpointManager
    {
        public MsmqSubscriberEndpointManager(MsmqSubscriberEndpoint endpoint)
            : base(endpoint)
        {
        }

        public void Create<T>()
        {
            throw new NotImplementedException();
        }
    }
}