using System;
using System.IO;
using System.Text;
using Mantle.BlobStorage.Interfaces;
using Mantle.Extensions;

namespace Mantle.BlobStorage.Extensions
{
    public static class BlobStorageClientExtensions
    {
        public static byte[] DownloadBytes(this IBlobStorageClient blobStorageClient, string blobName)
        {
            blobStorageClient.Require("blobStorageClient");
            blobName.Require("blobName");

            using (var outputStream = new MemoryStream())
            {
                blobStorageClient.DownloadBlob(blobName).CopyTo(outputStream);
                return outputStream.ToArray();
            }
        }

        public static void DownloadFile(this IBlobStorageClient blobStorageClient, string filePath, string blobName)
        {
            blobStorageClient.Require("blobStorageClient");
            filePath.Require("filePath");
            blobName.Require("blobName");

            using (var blobStream = blobStorageClient.DownloadBlob(blobName))
            using (var fileStream = File.Create(filePath))
            {
                blobStream.CopyTo(fileStream);
            }
        }

        public static T DownloadObject<T>(this IBlobStorageClient blobStorageClient, string blobName)
            where T : class
        {
            blobStorageClient.Require("blobStorageClient");
            blobName.Require("blobName");

            return blobStorageClient.DownloadText(blobName).FromJson<T>();
        }

        public static string DownloadText(this IBlobStorageClient blobStorageClient, string blobName)
        {
            blobStorageClient.Require("blobStorageClient");
            blobName.Require("blobName");

            using (var outputStream = new MemoryStream())
            {
                blobStorageClient.DownloadBlob(blobName).CopyTo(outputStream);
                return Encoding.UTF8.GetString(outputStream.ToArray());
            }
        }

        public static void UploadBytes(this IBlobStorageClient blobStorageClient, byte[] bytes, string blobName)
        {
            blobStorageClient.Require("blobStorageClient");
            bytes.Require("bytes");
            blobName.Require("blobName");

            if (bytes.Length == 0)
                throw new ArgumentException("Byte array is empty.", "bytes");

            blobStorageClient.UploadBlob(new MemoryStream(bytes), blobName);
        }

        public static void UploadFile(this IBlobStorageClient blobStorageClient, string filePath, string blobName)
        {
            blobStorageClient.Require("blobStorageClient");
            filePath.Require("filePath");
            blobName.Require("blobName");

            blobStorageClient.UploadBlob(new MemoryStream(File.ReadAllBytes(filePath)), blobName);
        }

        public static void UploadObject<T>(this IBlobStorageClient blobStorageClient, T @object, string blobName)
            where T : class
        {
            blobStorageClient.Require("blobStorageClient");
            @object.Require("object");
            blobName.Require("blobName");

            blobStorageClient.UploadText(@object.ToJson(), blobName);
        }

        public static void UploadText(this IBlobStorageClient blobStorageClient, string text, string blobName)
        {
            blobStorageClient.Require("blobStorageClient");
            text.Require("text");
            blobName.Require("blobName");

            blobStorageClient.UploadBlob(new MemoryStream(Encoding.UTF8.GetBytes(text)), blobName);
        }
    }
}