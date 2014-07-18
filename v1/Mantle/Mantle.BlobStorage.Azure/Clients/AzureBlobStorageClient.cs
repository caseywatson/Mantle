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

        public CloudBlobClient CloudBlobClient
        {
            get { return GetCloudBlobClient(); }
        }

        public CloudStorageAccount CloudStorageAccount
        {
            get { return GetCloudStorageAccount(); }
        }

        [Configurable(IsRequired = true)]
        public string ContainerName { get; set; }

        [Configurable(IsRequired = true)]
        public string StorageConnectionString { get; set; }

        public bool BlobExists(string blobName)
        {
            blobName.Require("blobName");

            CloudBlobContainer container = CloudBlobClient.GetContainerReference(ContainerName);

            if (container.Exists() == false)
                return false;

            return container.GetBlockBlobReference(blobName).Exists();
        }

        public void DeleteBlob(string blobName)
        {
            blobName.Require("blobName");

            CloudBlobContainer container = CloudBlobClient.GetContainerReference(ContainerName);

            if (container.Exists() == false)
                throw new InvalidOperationException(String.Format("Container [{0}] does not exist.", ContainerName));

            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

            if (blob.Exists() == false)
                throw new InvalidOperationException(String.Format("Blob [{0}/{1}] does not exist.", ContainerName,
                                                                  blobName));

            blob.Delete();
        }

        public Stream DownloadBlob(string blobName)
        {
            blobName.Require("blobName");

            CloudBlobContainer container = CloudBlobClient.GetContainerReference(ContainerName);

            if (container.Exists() == false)
                throw new InvalidOperationException(String.Format("Container [{0}] does not exist.", ContainerName));

            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

            if (blob.Exists() == false)
                throw new InvalidOperationException(String.Format("Blob [{0}/{1}] does not exist.", ContainerName,
                                                                  blobName));

            var stream = new MemoryStream();

            blob.DownloadToStream(stream);
            stream.TryToRewind();



            return stream;
        }

        public IEnumerable<string> ListBlobs()
        {
            CloudBlobContainer container = CloudBlobClient.GetContainerReference(ContainerName);

            if (container.Exists() == false)
                throw new InvalidOperationException(String.Format("Container [{0}] does not exist.", ContainerName));

            foreach (CloudBlockBlob blob in container.ListBlobs().OfType<CloudBlockBlob>())
                yield return blob.Name;
        }

        public void UploadBlob(Stream source, string blobName)
        {
            source.Require("source");
            blobName.Require("blobName");

            if (source.Length == 0)
                throw new ArgumentException("Source is empty.", "source");

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