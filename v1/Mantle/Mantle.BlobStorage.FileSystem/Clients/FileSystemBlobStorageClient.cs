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
            blobName.Require(nameof(blobName));

            if (Directory.Exists(Path) == false)
                return false;

            return (File.Exists(System.IO.Path.Combine(Path, blobName)));
        }

        public void DeleteBlob(string blobName)
        {
            blobName.Require(nameof(blobName));

            if (Directory.Exists(Path) == false)
                throw new InvalidOperationException($"Directory [{Path}] does not exist.");

            string filePath = System.IO.Path.Combine(Path, blobName);

            if (File.Exists(filePath) == false)
                throw new InvalidOperationException($"File [{filePath}] does not exist.");

            File.Delete(filePath);
        }

        public Stream DownloadBlob(string blobName)
        {
            blobName.Require(nameof(blobName));

            if (Directory.Exists(Path) == false)
                throw new InvalidOperationException($"Directory [{Path}] does not exist.");

            string filePath = System.IO.Path.Combine(Path, blobName);

            if (File.Exists(filePath) == false)
                throw new InvalidOperationException($"File [{filePath}] does not exist.");

            return (new MemoryStream(File.ReadAllBytes(filePath)) { Position = 0 });
        }

        public IEnumerable<string> ListBlobs()
        {
            if (Directory.Exists(Path) == false)
                throw new InvalidOperationException($"Directory [{Path}] does not exist.");

            return Directory.GetFiles(Path).Select(System.IO.Path.GetFileName);
        }

        public void UploadBlob(Stream blob, string blobName)
        {
            blob.Require(nameof(blob));
            blobName.Require(nameof(blobName));

            if (Directory.Exists(Path) == false)
                throw new InvalidOperationException($"Directory [{Path}] does not exist.");

            blob.TryToRewind();

            using (FileStream fileStream = File.Create(System.IO.Path.Combine(Path, blobName)))
                blob.CopyTo(fileStream);
        }
    }
}