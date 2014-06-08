using System;
using System.Collections.Generic;

namespace Mantle.Messaging
{
    public class PublisherEndpointDirectory : Dictionary<string, IPublisherEndpoint>, IPublisherEndpointDirectory
    {
        public PublisherEndpointDirectory(IPublisherEndpoint[] publisherEndpoints)
        {
            Load(publisherEndpoints);
        }

        private void Load(IPublisherEndpoint[] publisherEndpoints)
        {
            foreach (IPublisherEndpoint publisherEndpoint in publisherEndpoints)
            {
                if (ContainsKey(publisherEndpoint.Name))
                    throw new MessagingException(
                        String.Format("This directory already contains a publisher endpoint named [{0}].",
                                      publisherEndpoint.Name));

                publisherEndpoint.Validate();

                Add(publisherEndpoint.Name, publisherEndpoint);
            }
        }
    }
}