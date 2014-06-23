using System.Collections.Generic;
using System.IO;

namespace Mantle.BlobStorage.Interfaces
{
    public interface IBlobStorageClient
    {
        void DeleteBlob(string blobName);
        bool BlobExists(string blobName);
        void InsertOrUpdateBlob(Stream blob, string blobName);
        IEnumerable<string> ListBlobs();
        Stream LoadBlob(string blobName);
    }
}