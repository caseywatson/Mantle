using System;
using System.Collections.Generic;

namespace Mantle.Messaging
{
    public class CompositePublisherEndpoint : Endpoint, IPublisherEndpoint
    {
        private readonly IPublisherEndpointDirectory publisherDirectory;

        public CompositePublisherEndpoint(IPublisherEndpointDirectory publisherDirectory)
        {
            this.publisherDirectory = publisherDirectory;
            ChildEndpoints = new Dictionary<string, IPublisherEndpoint>();
        }

        public Dictionary<string, IPublisherEndpoint> ChildEndpoints { get; set; }

        public IPublisherClient GetClient()
        {
            return new CompositePublisherClient(this);
        }

        public override void Validate()
        {
            base.Validate();

            if (ChildEndpoints.Count == 0)
                throw new MessagingException(
                    "Composite publisher endpoint must contain at least one child endpoint.");

            foreach (string endpointKey in ChildEndpoints.Keys)
                ChildEndpoints[endpointKey].Validate();
        }

        public void Setup(string name, params string[] childEndpointNames)
        {
            Name = name;

            if ((childEndpointNames == null) || (childEndpointNames.Length == 0))
                throw new MessagingException("At least one child endpoint name is required.");

            foreach (string endpointName in childEndpointNames)
            {
                if (publisherDirectory.ContainsKey(endpointName) == false)
                    throw new MessagingException(String.Format("The specified child endpoint [{0}] was not found.",
                        endpointName));

                if (ChildEndpoints.ContainsKey(endpointName))
                    throw new MessagingException(
                        String.Format("The specified child endpoint [{0}] has already been added.", endpointName));

                ChildEndpoints.Add(endpointName, publisherDirectory[endpointName]);
            }

            Validate();
        }
    }
}