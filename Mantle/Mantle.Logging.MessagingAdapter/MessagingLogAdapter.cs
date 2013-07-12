using System;
using Mantle.Messaging;

namespace Mantle.Logging.MessagingAdapter
{
    public class MessagingLogAdapter : ILogAdapter
    {
        private readonly IPublisherClient publisherClient;

        public MessagingLogAdapter(IPublisherClient publisherClient)
        {
            if (publisherClient == null)
                throw new ArgumentNullException("publisherClient");

            this.publisherClient = publisherClient;
        }

        public void Record(Event evt)
        {
            if (evt == null)
                throw new ArgumentNullException("evt");

            publisherClient.Publish(evt);
        }
    }
}