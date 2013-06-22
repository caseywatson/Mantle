using System;
using Mantle.Hosting;
using Mantle.Messaging;
using Mantle.Storage;

namespace Mantle.Samples.Simple.Worker
{
    public class SimpleWorker : BaseWorker
    {
        // This very simple worker class processes a message from the "SimpleWorkerInput" subscriber endpoint,
        // saves the message to "SimpleStorage" storage then forwards the message to the "SimpleWorkerOutput" publisher endpoint.

        private readonly IPublisherEndpointDirectory publisherDirectory;
        private readonly IStorageClientDirectory storageDirectory;
        private readonly ISubscriberEndpointDirectory subscriberDirectory;

        public SimpleWorker(IPublisherEndpointDirectory publisherDirectory, IStorageClientDirectory storageDirectory,
                            ISubscriberEndpointDirectory subscriberDirectory)
        {
            this.storageDirectory = storageDirectory;
            this.publisherDirectory = publisherDirectory;
            this.subscriberDirectory = subscriberDirectory;
        }

        public override void Start()
        {
            IStorageClient storageClient = storageDirectory["SimpleStorage"];

            IPublisherClient publisherClient = publisherDirectory["SimpleWorkerOutput"].GetClient();
            ISubscriberClient subscriberClient = subscriberDirectory["SimpleWorkerInput"].GetClient();

            while (true)
            {
                // Retrieve the next message from the subscription queue.
                // The message is not *actually* removed - it is temporarily
                // made invisible to other subscribers. This subscriber has a 
                // time window (configured at the infrastructure level) that
                // determines how long this worker has to process the incoming message.
                // If the message is not processed in the window it will become
                // visible once again.

                Message<Customer> incomingMessage = subscriberClient.Receive<Customer>();

                // If no message was available continue back to the top of the loop
                // and check again.

                if (incomingMessage == null)
                    continue;

                try
                {
                    // The actual message retrieved is a Message<Customer>.
                    // Retrieve the payload of the message (the Customer).

                    Customer customer = incomingMessage.Payload;

                    // Serialize the customer object and save it to storage.

                    storageClient.SaveObject(customer.Serialize(), customer.Id);

                    // Indicate that the customer has been saved.

                    OnMessageOccurred("Customer [{0}] has been saved.", customer.Id);

                    // Forward the message to the publisher queue.

                    publisherClient.Publish(customer);

                    // Finally, complete the message so that it will be removed from
                    // the subscription queue and not be made available to other subscribers.

                    incomingMessage.Complete();
                }
                catch (Exception ex)
                {
                    // If something went wrong, abandon the message. Abandoning the
                    // message returns it to the subscriber queue where it will potentially
                    // be picked up by another subscriber.

                    incomingMessage.Abandon();

                    // Indicate that a problem has occurred...

                    OnErrorOccurred(
                        "An error occurred while attempting to process a Customer. See below for additional details:\n\n{0}",
                        ex.Message);

                    throw;
                }
            }
        }

        public override void Stop()
        {
            // Do any necessary clean up work here.
        }
    }
}