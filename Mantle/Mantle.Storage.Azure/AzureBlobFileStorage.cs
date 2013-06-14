using System;
using System.IO;
using System.Linq;
using Mantle.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Mantle.Storage.Azure
{
    public class AzureBlobFileStorage : IFileStorage
    {
        private readonly CloudBlobClient cloudBlobClient;
        private readonly CloudStorageAccount cloudStorageAccount;

        public AzureBlobFileStorage(IAzureStorageConfiguration configuration)
        {
            cloudStorageAccount = CloudStorageAccount.Parse(configuration.ConnectionString);
            cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
        }

        public string ContainerName { get; set; }

        public bool Exists(string fileName)
        {
            BlobLocation blobLocation = BuildLocation(fileName);
            CloudBlobContainer containerReference = cloudBlobClient.GetContainerReference(blobLocation.ContainerName);

            if (containerReference.Exists() == false)
                return false;

            CloudBlockBlob blobReference = containerReference.GetBlockBlobReference(blobLocation.BlobName);

            return blobReference.Exists();
        }

        public Stream Load(string fileName)
        {
            BlobLocation blobLocation = BuildLocation(fileName);
            CloudBlobContainer containerReference = cloudBlobClient.GetContainerReference(blobLocation.ContainerName);

            if (containerReference.Exists() == false)
                throw new InvalidOperationException(
                    String.Format("Azure blob container [{0}] does not exist. File not found.",
                                  blobLocation.ContainerName));

            CloudBlockBlob blobReference = containerReference.GetBlockBlobReference(blobLocation.BlobName);

            if (blobReference.Exists() == false)
                throw new InvalidOperationException(String.Format(
                    "Azure blob [{0}/{1}] does not exist. File not found.", blobLocation.ContainerName,
                    blobLocation.BlobName));

            var outputStream = new MemoryStream();

            blobReference.DownloadToStream(outputStream);

            return outputStream;
        }

        public void Save(Stream fileContents, string fileName)
        {
            if (fileContents == null)
                throw new ArgumentNullException("fileContents");

            BlobLocation blobLocation = BuildLocation(fileName);
            CloudBlobContainer containerReference = cloudBlobClient.GetContainerReference(blobLocation.ContainerName);

            containerReference.CreateIfNotExists();

            CloudBlockBlob blobReference = containerReference.GetBlockBlobReference(blobLocation.BlobName);

            blobReference.UploadFromStream(fileContents);
        }

        private BlobLocation BuildLocation(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name is required.", "fileName");

            fileName = fileName.Replace('\\', '/');

            if (String.IsNullOrEmpty(ContainerName) == false)
                return new BlobLocation {ContainerName = ContainerName.ToLower(), BlobName = fileName};

            string[] fileNameParts = fileName.Split('/').Where(p => (p.Trim().Length > 0)).ToArray();

            if (fileNameParts.Length == 1)
                throw new ArgumentException(
                    "Container name not specified. The expected format is [{Container Name}/{Blob Name}].", "fileName");

            return new BlobLocation
                {
                    ContainerName = ContainerName.ToLower(),
                    BlobName = String.Join("/", fileNameParts.Skip(1))
                };
        }

        private class BlobLocation
        {
            public string ContainerName { get; set; }
            public string BlobName { get; set; }
        }
    }
}