using System;
using System.Linq;
using Mantle.BlobStorage.Extensions;
using Mantle.BlobStorage.Interfaces;
using Mantle.Cache.Interfaces;
using Mantle.DictionaryStorage.Interfaces;
using Mantle.Hosting.Workers;
using Mantle.Interfaces;
using Mantle.Messaging.Interfaces;
using Mantle.Messaging.Messages;
using Mantle.Sample.PublisherConsole.Module.Models;

namespace Mantle.Sample.PublisherConsole.Module.Workers
{
    public class PublisherWorker : BaseWorker
    {
        private readonly IDirectory<IBlobStorageClient> blobStorageClients;
        private readonly IDirectory<ICacheClient<SampleModel>> cacheClients;
        private readonly IDirectory<IDictionaryStorageClient<SampleModel>> dictionaryStorageClients;
        private readonly IDirectory<IPublisherChannel<MessageEnvelope>> publisherChannels;
        private readonly ISerializer<SampleModel> sampleModelSerializer;
        private readonly ITypeTokenProvider[] typeTokenProviders;

        public PublisherWorker(IDirectory<IBlobStorageClient> blobStorageClients,
                               IDirectory<ICacheClient<SampleModel>> cacheClients,
                               IDirectory<IDictionaryStorageClient<SampleModel>> dictionaryStorageClients,
                               IDirectory<IPublisherChannel<MessageEnvelope>> publisherChannels,
                               ISerializer<SampleModel> sampleModelSerializer,
                               ITypeTokenProvider[] typeTokenProviders)
        {
            this.blobStorageClients = blobStorageClients;
            this.cacheClients = cacheClients;
            this.dictionaryStorageClients = dictionaryStorageClients;
            this.publisherChannels = publisherChannels;
            this.sampleModelSerializer = sampleModelSerializer;
            this.typeTokenProviders = typeTokenProviders;
        }

        public override void Start()
        {
            // RunDictionaryStorageIntegrationTests();
            // RunMessagingIntegrationTests();
            // RunCachingIntegrationTests();
            RunBlobStorageIntegrationTests();
        }

        public override void Stop()
        {
            throw new NotImplementedException();
        }

        private SampleModel CreateSampleModel()
        {
            var model = new SampleModel();

            model.SampleBool = true;
            model.SampleByte = byte.MaxValue;
            model.SampleDateTime = DateTime.Parse("1/11/1981");
            model.SampleDecimal = 1M;
            model.SampleDouble = double.MaxValue;
            model.SampleFloat = float.MaxValue;
            model.SampleGuid = Guid.NewGuid();
            model.SampleInt = int.MaxValue;
            model.SampleLong = long.MaxValue;
            model.SampleNullableBool = false;
            model.SampleNullableDateTime = DateTime.Parse("1/11/2014");
            model.SampleNullableDecimal = -1M;
            model.SampleNullableDouble = double.MinValue;
            model.SampleNullableGuid = Guid.NewGuid();
            model.SampleNullableLong = long.MinValue;
            model.SampleObject = new SampleModel {SampleString = "Eureka!"};
            model.SampleString = "What up?";

            return model;
        }

        private void RunBlobStorageIntegrationTests()
        {
            const string blobName = "SampleModel";

            var sampleModel = CreateSampleModel();
            var blobStorageClient = blobStorageClients["AzureBlobs"];

            blobStorageClient.UploadObject(sampleModel, blobName);

            Console.WriteLine(blobStorageClient.BlobExists(blobName));

            foreach (var listBlobName in blobStorageClient.ListBlobs())
                Console.WriteLine(listBlobName);

            sampleModel = blobStorageClient.DownloadObject<SampleModel>(blobName);

            blobStorageClient.DeleteBlob(blobName);
        }

        private void RunCachingIntegrationTests()
        {
            const string cacheId = "TestCache";

            var sampleModel = CreateSampleModel();
            var cache = cacheClients["AzManagedCache"];

            cache.Put(sampleModel, cacheId);

            sampleModel = cache.Get(cacheId);

            Console.WriteLine(sampleModel.SampleString);
        }

        private void RunDictionaryStorageIntegrationTests()
        {
            var dictionaryStorageClient = dictionaryStorageClients["AzDictionary"];
            var sampleModel = CreateSampleModel();

            var partitionId = "MantleTesting";
            var entityId = Guid.NewGuid().ToString();

            dictionaryStorageClient.InsertOrUpdateEntity(sampleModel, entityId, partitionId);

            sampleModel = dictionaryStorageClient.LoadEntity(Guid.NewGuid().ToString(), partitionId);
            sampleModel = dictionaryStorageClient.LoadEntity(entityId, partitionId);

            Console.WriteLine(sampleModel.SampleString);

            var sampleModels = dictionaryStorageClient.LoadAllEntities(partitionId).ToList();

            Console.WriteLine(sampleModels.Count);

            dictionaryStorageClient.DeleteEntity(entityId, partitionId);
        }

        private void RunMessagingIntegrationTests()
        {
            var sampleModel = CreateSampleModel();

            var serviceBusQueuePublisher = publisherChannels["AzServiceBusQueue"];
            var serviceBusTopicPublisher = publisherChannels["AzServiceBusTopic"];
            var storageQueuePublisher = publisherChannels["AzStorageQueue"];

            var envelope = new MessageEnvelope();

            envelope.Body = sampleModelSerializer.Serialize(sampleModel);
            envelope.BodyTypeTokens = typeTokenProviders.Select(ttp => ttp.GetTypeToken<SampleModel>()).ToList();
            envelope.CorrelationId = Guid.NewGuid().ToString();
            envelope.Id = Guid.NewGuid().ToString();
            envelope.TimeToLive = 10;

            serviceBusQueuePublisher.Publish(envelope);
            serviceBusTopicPublisher.Publish(envelope);
            storageQueuePublisher.Publish(envelope);
        }
    }
}