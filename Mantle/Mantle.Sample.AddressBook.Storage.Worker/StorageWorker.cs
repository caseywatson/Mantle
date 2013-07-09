using System;
using Mantle.Hosting;
using Mantle.Messaging;
using Mantle.Sample.AddressBook.Shared;
using Mantle.Storage;

namespace Mantle.Sample.AddressBook.Storage.Worker
{
    // TOOD: Give your worker a meaningful name.

    public class Worker : BaseWorker
    {
        private readonly IPublisherEndpointDirectory publisherDirectory;
        private readonly IStorageClientDirectory storageDirectory;
        private readonly ISubscriberEndpointDirectory subscriberDirectory;

        public Worker(IPublisherEndpointDirectory publisherDirectory, IStorageClientDirectory storageDirectory,
                      ISubscriberEndpointDirectory subscriberDirectory)
        {
            this.publisherDirectory = publisherDirectory;
            this.storageDirectory = storageDirectory;
            this.subscriberDirectory = subscriberDirectory;
        }

        public override void Start()
        {
            IPublisherClient publisherClient = publisherDirectory["My Publisher Endpoint"].GetClient();
            IStorageClient storageClient = storageDirectory["My Storage Client"];
            ISubscriberClient subscriberClient = subscriberDirectory["My Subscriber Endpoint"].GetClient();

            while (true)
            {
                // TODO: Add your application-specific domain logic here.

                OnMessageOccurred("Waiting for the next Person message...");
                Message<Person> personMessage = subscriberClient.Receive<Person>();

                if (personMessage == null)
                    continue;

                try
                {
                    Person person = personMessage.Payload;
                    OnMessageOccurred("Received Person message [{0}].", person.Id);
                    storageClient.SaveObject(person.Serialize(), person.Id);
                    OnMessageOccurred("Saved Person [{0}].", person.Id);
                    personMessage.DoIfImplements<ICanBeCompleted>(m => m.Complete());
                }
                catch (Exception ex)
                {
                    personMessage.DoIfImplements<ICanBeAbandoned>(m => m.Abandon());
                    OnErrorOccurred(
                        "An error occurred while processing an incoming Person message. See below for additional details: \n\n{0}",
                        ex.Message);
                }
            }
        }

        public override void Stop()
        {
        }
    }
}