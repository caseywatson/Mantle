using System;
using System.Collections.Generic;

namespace Mantle.Messaging
{
    public class SubscriberEndpointDirectory : Dictionary<string, ISubscriberEndpoint>, ISubscriberEndpointDirectory
    {
        public SubscriberEndpointDirectory(ISubscriberEndpoint[] subscriberEndpoints)
        {
            if (subscriberEndpoints == null)
                throw new ArgumentNullException("subscriberEndpoints");

            Load(subscriberEndpoints);
        }

        private void Load(ISubscriberEndpoint[] subscriberEndpoints)
        {
            foreach (ISubscriberEndpoint subscriberEndpoint in subscriberEndpoints)
            {
                if (ContainsKey(subscriberEndpoint.Name))
                    throw new MessagingException(
                        String.Format("This directory already contains a subscriber endpoint named [{0}].",
                                      subscriberEndpoint.Name));

                subscriberEndpoint.Validate();

                Add(subscriberEndpoint.Name, subscriberEndpoint);
            }
        }
    }
}