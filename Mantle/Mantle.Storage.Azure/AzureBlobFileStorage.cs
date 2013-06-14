using System;
using System.IO;
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
            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name is required.", "fileName");

            CloudBlobContainer containerReference = cloudBlobClient.GetContainerReference(ContainerName.ToLower());

            if (containerReference.Exists() == false)
                return false;

            CloudBlockBlob blobReference = containerReference.GetBlockBlobReference(fileName);

            return blobReference.Exists();
        }

        public Stream Load(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentException("fileName");

            CloudBlobContainer containerReference = cloudBlobClient.GetContainerReference(ContainerName.ToLower());

            if (containerReference.Exists() == false)
                throw new InvalidOperationException(
                    String.Format("Azure blob container [{0}] does not exist. File not found.",
                                  ContainerName));

            CloudBlockBlob blobReference = containerReference.GetBlockBlobReference(fileName);

            if (blobReference.Exists() == false)
                throw new InvalidOperationException(
                    String.Format("Azure blob [{0}/{1}] does not exist. File not found.",
                                  ContainerName, fileName));

            var outputStream = new MemoryStream();

            blobReference.DownloadToStream(outputStream);

            return outputStream;
        }

        public void Save(Stream fileContents, string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name is required.");

            if (fileContents == null)
                throw new ArgumentNullException("fileContents");

            CloudBlobContainer containerReference = cloudBlobClient.GetContainerReference(ContainerName.ToLower());

            containerReference.CreateIfNotExists();

            CloudBlockBlob blobReference = containerReference.GetBlockBlobReference(fileName);

            blobReference.UploadFromStream(fileContents);
        }
    }
}