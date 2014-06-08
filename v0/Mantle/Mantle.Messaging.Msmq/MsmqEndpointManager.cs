using System;
using System.Messaging;

namespace Mantle.Messaging.Msmq
{
    public class MsmqEndpointManager
    {
        private readonly MsmqEndpoint endpoint;

        public MsmqEndpointManager(MsmqEndpoint endpoint)
        {
            if (endpoint == null)
                throw new ArgumentNullException("endpoint");

            endpoint.Validate();

            this.endpoint = endpoint;
        }

        public bool DoesExist()
        {
            return MessageQueue.Exists(endpoint.QueuePath);
        }

        public void Create()
        {
            MessageQueue.Create(endpoint.QueuePath);
        }
    }
}