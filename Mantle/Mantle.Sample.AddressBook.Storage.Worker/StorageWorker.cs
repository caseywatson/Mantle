using System;
using Mantle.Hosting;
using Mantle.Messaging;
using Mantle.Sample.AddressBook.Shared;
using Mantle.Storage;

namespace Mantle.Sample.AddressBook.Storage.Worker
{
    public class StorageWorker : BaseWorker
    {
        private readonly IStorageClientDirectory storageDirectory;
        private readonly ISubscriberEndpointDirectory subscriberDirectory;

        public StorageWorker(IStorageClientDirectory storageDirectory, ISubscriberEndpointDirectory subscriberDirectory)
        {
            this.storageDirectory = storageDirectory;
            this.subscriberDirectory = subscriberDirectory;
        }

        public override void Start()
        {
            IStorageClient storageClient = storageDirectory["PersonStorage"];
            ISubscriberClient subscriberClient = subscriberDirectory["PersonQueue"].GetClient();

            while (true)
            {
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
                    personMessage.Complete();
                }
                catch (Exception ex)
                {
                    personMessage.Abandon();
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