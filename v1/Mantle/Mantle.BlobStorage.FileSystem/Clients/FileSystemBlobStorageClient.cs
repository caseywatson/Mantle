using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mantle.BlobStorage.Interfaces;
using Mantle.Configuration.Attributes;
using Mantle.Extensions;

namespace Mantle.BlobStorage.FileSystem.Clients
{
    public class FileSystemBlobStorageClient : IBlobStorageClient
    {
        [Configurable(IsRequired = true)]
        public string Path { get; set; }

        public bool BlobExists(string blobName)
        {
            blobName.Require("blobName");

            if (Directory.Exists(Path) == false)
                return false;

            return (File.Exists(System.IO.Path.Combine(Path, blobName)));
        }

        public void DeleteBlob(string blobName)
        {
            blobName.Require("blobName");

            if (Directory.Exists(Path) == false)
                throw new InvalidOperationException(String.Format("Directory [{0}] does not exist.", Path));

            string filePath = System.IO.Path.Combine(Path, blobName);

            if (File.Exists(filePath) == false)
                throw new InvalidOperationException(String.Format("File [{0}] does not exist.", filePath));

            File.Delete(filePath);
        }

        public Stream DownloadBlob(string blobName)
        {
            blobName.Require("blobName");

            if (Directory.Exists(Path) == false)
                throw new InvalidOperationException(String.Format("Directory [{0}] does not exist.", Path));

            string filePath = System.IO.Path.Combine(Path, blobName);

            if (File.Exists(filePath) == false)
                throw new InvalidOperationException(String.Format("File [{0}] does not exist.", filePath));

            return (new MemoryStream(File.ReadAllBytes(filePath)) {Position = 0});
        }

        public IEnumerable<string> ListBlobs()
        {
            if (Directory.Exists(Path) == false)
                throw new InvalidOperationException(String.Format("Directory [{0}] does not exist.", Path));

            return Directory.GetFiles(Path).Select(System.IO.Path.GetFileName);
        }

        public void UploadBlob(Stream blob, string blobName)
        {
            blob.Require("blob");
            blobName.Require("blobName");

            if (Directory.Exists(Path) == false)
                throw new InvalidOperationException(String.Format("Directory [{0}] does not exist.", Path));

            blob.Rewind();

            using (FileStream fileStream = File.Create(System.IO.Path.Combine(Path, blobName)))
                blob.CopyTo(fileStream);
        }
    }
}