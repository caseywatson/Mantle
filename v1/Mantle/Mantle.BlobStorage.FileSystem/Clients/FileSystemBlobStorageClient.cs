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

        public bool BlobExists(string blobName)
        {
            blobName.Require("blobName");

            if (Directory.Exists(Path) == false)
                return false;

            return (File.Exists(System.IO.Path.Combine(Path, blobName)));
        }

        public void InsertOrUpdateBlob(Stream blob, string blobName)
        {
            blob.Require("blob");
            blobName.Require("blobName");

            if (Directory.Exists(Path) == false)
                throw new InvalidOperationException(String.Format("Directory [{0}] does not exist.", Path));

            if (blob.CanSeek)
                blob.Position = 0;

            using (FileStream fileStream = File.Create(System.IO.Path.Combine(Path, blobName)))
                blob.CopyTo(fileStream);
        }

        public IEnumerable<string> ListBlobs()
        {
            if (Directory.Exists(Path) == false)
                throw new InvalidOperationException(String.Format("Directory [{0}] does not exist.", Path));

            return Directory.GetFiles(Path).Select(System.IO.Path.GetFileName);
        }

        public Stream LoadBlob(string blobName)
        {
            blobName.Require("blobName");

            if (Directory.Exists(Path) == false)
                throw new InvalidOperationException(String.Format("Directory [{0}] does not exist.", Path));

            string filePath = System.IO.Path.Combine(Path, blobName);

            if (File.Exists(filePath) == false)
                throw new InvalidOperationException(String.Format("File [{0}] does not exist.", filePath));

            return (new MemoryStream(File.ReadAllBytes(filePath)) {Position = 0});
        }
    }
}