using System.Collections.Generic;
using System.IO;

namespace Mantle.BlobStorage.Interfaces
{
    public interface IBlobStorageClient
    {
        bool BlobExists(string blobName);
        void DeleteBlob(string blobName);
        Stream DownloadBlob(string blobName);
        IEnumerable<string> ListBlobs();
        void UploadBlob(Stream blob, string blobName);
    }
}