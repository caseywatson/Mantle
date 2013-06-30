using System;
using System.Collections.Generic;

namespace Mantle.Messaging
{
    public class CompositePublisherClient : IPublisherClient
    {
        private readonly List<IPublisherClient> publisherClients;

        public CompositePublisherClient(CompositePublisherEndpoint endpoint)
        {
            if (endpoint == null)
                throw new ArgumentNullException("endpoint");

            endpoint.Validate();

            publisherClients = new List<IPublisherClient>();

            foreach (string endpointKey in endpoint.ChildEndpoints.Keys)
                publisherClients.Add(endpoint.ChildEndpoints[endpointKey].GetClient());
        }

        public void Publish<T>(T message)
        {
            foreach (IPublisherClient publisherClient in publisherClients)
                publisherClient.Publish(message);
        }
    }
}