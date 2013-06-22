using System;
using System.IO;
using System.Linq;
using Mantle.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Mantle.Storage.Azure
{
    public class AzureBlobStorageClient : BaseStorageClient, IStorageClient
    {
        private readonly CloudBlobClient cloudBlobClient;
        private readonly CloudStorageAccount cloudStorageAccount;

        public AzureBlobStorageClient(IAzureStorageConfiguration configuration)
        {
            configuration.Validate();

            cloudStorageAccount = CloudStorageAccount.Parse(configuration.ConnectionString);
            cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
        }

        public string ContainerName { get; set; }

        public bool DoesObjectExist(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name is required.", "fileName");

            try
            {
                CloudBlobContainer containerReference = cloudBlobClient.GetContainerReference(ContainerName.ToLower());

                if (containerReference.Exists() == false)
                    return false;

                CloudBlockBlob blobReference = containerReference.GetBlockBlobReference(fileName);

                return blobReference.Exists();
            }
            catch (Exception ex)
            {
                throw new StorageException("An error occurred while processing your request.", ex);
            }
        }

        public Stream LoadObject(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentException("fileName");

            try
            {
                CloudBlobContainer containerReference = cloudBlobClient.GetContainerReference(ContainerName.ToLower());

                if (containerReference.Exists() == false)
                    throw new StorageException(
                        String.Format("Azure blob container [{0}] does not exist. File not found.", ContainerName));

                CloudBlockBlob blobReference = containerReference.GetBlockBlobReference(fileName);

                if (blobReference.Exists() == false)
                    throw new StorageException(String.Format("Azure blob [{0}/{1}] does not exist. File not found.",
                                                             ContainerName, fileName));

                var outputStream = new MemoryStream();

                blobReference.DownloadToStream(outputStream);

                return outputStream;
            }
            catch (Exception ex)
            {
                throw new StorageException("An error occured while processing your request.", ex);
            }
        }

        public void SaveObject(Stream fileContents, string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name is required.");

            if (fileContents == null)
                throw new ArgumentNullException("fileContents");

            try
            {
                CloudBlobContainer containerReference = cloudBlobClient.GetContainerReference(ContainerName.ToLower());

                containerReference.CreateIfNotExists();

                CloudBlockBlob blobReference = containerReference.GetBlockBlobReference(fileName);

                blobReference.UploadFromStream(fileContents);
            }
            catch (Exception ex)
            {
                throw new StorageException("An error occurred while processing your request.", ex);
            }
        }

        public string[] ListObjects()
        {
            try
            {
                CloudBlobContainer containerReference = cloudBlobClient.GetContainerReference(ContainerName.ToLower());

                if (containerReference.Exists() == false)
                    throw new StorageException(
                        String.Format("Azure blob container [{0}] does not exist. File not found.", ContainerName));

                return containerReference.ListBlobs().OfType<CloudBlockBlob>().Select(b => (b.Name)).ToArray();
            }
            catch (Exception ex)
            {
                throw new StorageException("An error occured while processing your request.", ex);
            }
        }

        public override void Validate()
        {
            base.Validate();

            if (String.IsNullOrEmpty(ContainerName))
                throw new StorageException("Container name not provided.");

            if (ContainerName.Where(Char.IsLetter).All(Char.IsLower) == false)
                throw new StorageException("Container name must contain only lower-case letters.");

            if (ContainerName.All(c => (Char.IsLetterOrDigit(c)) || (c == '-') || (c == '.')) == false)
                throw new StorageException(
                    "Container name must contain only letters (A-Z), digits (0-9), periods (.) and hyphens (-).");

            if (Char.IsLetterOrDigit(ContainerName[0]) == false)
                throw new StorageException("Container name must start with either a letter (A-Z) or digit (0-9).");

            if (Char.IsLetterOrDigit(ContainerName.Last()) == false)
                throw new StorageException("Container name must end with either a letter (A-Z) or digit (0-9).");

            if (ContainerName.Contains(".."))
                throw new StorageException("Container name must not contain repeating periods (..).");

            if (ContainerName.Contains("--"))
                throw new StorageException("Container name must not contain repeating hyphens (--).");

            if (ContainerName.Contains("-.") || ContainerName.Contains(".-"))
                throw new StorageException("Container name must not contain (-.) or (.-).");

            if ((ContainerName.Length < 3) || (ContainerName.Length > 63))
                throw new StorageException("Container name must be between 3 and 63 characters in length.");
        }

        public void Setup(string name, string containerName)
        {
            Name = name;
            ContainerName = containerName;

            Validate();
        }
    }
}