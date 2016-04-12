using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mantle.BlobStorage.Interfaces;
using Mantle.Configuration.Attributes;
using Mantle.Extensions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Mantle.BlobStorage.Azure.Clients
{
    public class AzureBlobStorageClient : IBlobStorageClient
    {
        private CloudBlobClient cloudBlobClient;
        private CloudStorageAccount cloudStorageAccount;

        public CloudBlobClient CloudBlobClient => GetCloudBlobClient();

        public CloudStorageAccount CloudStorageAccount => GetCloudStorageAccount();

        [Configurable(IsRequired = true)]
        public string ContainerName { get; set; }

        [Configurable(IsRequired = true)]
        public string StorageConnectionString { get; set; }

        public bool BlobExists(string blobName)
        {
            blobName.Require(nameof(blobName));

            CloudBlobContainer container = CloudBlobClient.GetContainerReference(ContainerName);

            if (container.Exists() == false)
                return false;

            return container.GetBlockBlobReference(blobName).Exists();
        }

        public void DeleteBlob(string blobName)
        {
            blobName.Require(nameof(blobName));

            CloudBlobContainer container = CloudBlobClient.GetContainerReference(ContainerName);

            if (container.Exists() == false)
                throw new InvalidOperationException($"Container [{ContainerName}] does not exist.");

            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

            if (blob.Exists() == false)
                throw new InvalidOperationException($"Blob [{ContainerName}/{blobName}] does not exist.");

            blob.Delete();
        }

        public Stream DownloadBlob(string blobName)
        {
            blobName.Require(nameof(blobName));

            CloudBlobContainer container = CloudBlobClient.GetContainerReference(ContainerName);

            if (container.Exists() == false)
                throw new InvalidOperationException($"Container [{ContainerName}] does not exist.");

            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

            if (blob.Exists() == false)
                throw new InvalidOperationException($"Blob [{ContainerName}/{blobName}] does not exist.");

            var stream = new MemoryStream();    

            blob.DownloadToStream(stream);
            stream.TryToRewind();

            return stream;
        }

        public IEnumerable<string> ListBlobs()
        {
            CloudBlobContainer container = CloudBlobClient.GetContainerReference(ContainerName);

            if (container.Exists() == false)
                throw new InvalidOperationException($"Container [{ContainerName}] does not exist.");

            foreach (CloudBlockBlob blob in container.ListBlobs().OfType<CloudBlockBlob>())
                yield return blob.Name;
        }

        public void UploadBlob(Stream source, string blobName)
        {
            source.Require(nameof(source));
            blobName.Require(nameof(blobName));

            if (source.Length == 0)
                throw new ArgumentException($"[{nameof(source)}] is empty.", nameof(source));

            source.TryToRewind();

            CloudBlobContainer container = CloudBlobClient.GetContainerReference(ContainerName);

            container.CreateIfNotExists();

            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

            blob.UploadFromStream(source);
        }

        private CloudBlobClient GetCloudBlobClient()
        {
            return (cloudBlobClient = (cloudBlobClient ??
                                       CloudStorageAccount.CreateCloudBlobClient()));
        }

        private CloudStorageAccount GetCloudStorageAccount()
        {
            return (cloudStorageAccount = (cloudStorageAccount ??
                                           CloudStorageAccount.Parse(StorageConnectionString)));
        }
    }
}