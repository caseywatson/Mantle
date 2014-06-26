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

        public CloudStorageAccount CloudStorageAccount
        {
            get
            {
                return (cloudStorageAccount = (cloudStorageAccount ??
                                               CloudStorageAccount.Parse(StorageConnectionString)));
            }
        }

        public CloudBlobClient CloudBlobClient
        {
            get
            {
                return (cloudBlobClient = (cloudBlobClient ??
                                           CloudStorageAccount.CreateCloudBlobClient()));
            }
        }

        [Configurable(IsRequired = true)]
        public string ContainerName { get; set; }

        [Configurable(IsRequired = true)]
        public string StorageConnectionString { get; set; }

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

        public bool BlobExists(string blobName)
        {
            blobName.Require("blobName");

            CloudBlobContainer container = CloudBlobClient.GetContainerReference(ContainerName);

            if (container.Exists() == false)
                return false;

            return container.GetBlockBlobReference(blobName).Exists();
        }

        public void UploadBlob(Stream source, string blobName)
        {
            source.Require("source");
            blobName.Require("blobName");

            if (source.Length == 0)
                throw new ArgumentException("Source is empty.", "source");

            source.Rewind();

            CloudBlobContainer container = CloudBlobClient.GetContainerReference(ContainerName);

            container.CreateIfNotExists();

            CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

            blob.UploadFromStream(source);
        }

        public IEnumerable<string> ListBlobs()
        {
            CloudBlobContainer container = CloudBlobClient.GetContainerReference(ContainerName);

            if (container.Exists() == false)
                throw new InvalidOperationException(String.Format("Container [{0}] does not exist.", ContainerName));

            foreach (CloudBlockBlob blob in container.ListBlobs().OfType<CloudBlockBlob>())
                yield return blob.Name;
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
            stream.Rewind();

            return stream;
        }
    }
}