using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Amazon;
using Amazon.S3;
using Amazon.S3.Util;
using Mantle.Aws.Tests.Constants;
using Mantle.BlobStorage.Aws.Clients;
using Mantle.Extensions;
using Mantle.FaultTolerance.Strategies;
using NUnit.Framework;

namespace Mantle.Aws.Tests
{
    [TestFixture]
    public class AwsS3BlobStorageClientTests
    {
        [TearDown]
        public void TearDown()
        {
            using (var client = new AmazonS3Client(AwsTestConfiguration.AwsAccessKeyId,
                                                   AwsTestConfiguration.AwsSecretAccessKey,
                                                   GetRegionEndpoint()))
            {
                if (client.ListBuckets().Buckets.Any(b => b.BucketName == bucketName))
                    AmazonS3Util.DeleteS3BucketWithObjects(client, bucketName);
            }
        }

        private const int BlobSize = (5 * 1024);

        private readonly string bucketName;

        public AwsS3BlobStorageClientTests()
        {
            bucketName = Guid.NewGuid().ToString();
        }

        private AwsS3BlobStorageClient CreateBlobStorageClient()
        {
            var tfStrategy = new NoTransientFaultStrategy();
            var regionEndpoints = new AwsRegionEndpoints(tfStrategy);

            return new AwsS3BlobStorageClient(regionEndpoints, tfStrategy)
            {
                AutoSetup = AwsTestConfiguration.AutoSetup,
                AwsAccessKeyId = AwsTestConfiguration.AwsAccessKeyId,
                AwsRegionName = AwsTestConfiguration.AwsRegionName,
                AwsSecretAccessKey = AwsTestConfiguration.AwsSecretAccessKey,
                BucketName = bucketName
            };
        }

        private byte[] GenerateRandomByteArray(int size)
        {
            var byteArray = new byte[size];
            var randomNumberGenerator = new RNGCryptoServiceProvider();

            randomNumberGenerator.GetBytes(byteArray);

            return byteArray;
        }

        private RegionEndpoint GetRegionEndpoint()
        {
            return RegionEndpoint.GetBySystemName(AwsTestConfiguration.AwsRegionName);
        }

        [Test]
        public void Should_throw_an_exception_when_trying_to_download_a_blob_with_an_empty_name()
        {
            var blobStorageClient = CreateBlobStorageClient();

            Assert.Throws<ArgumentException>(() => blobStorageClient.DownloadBlob(string.Empty));
        }

        [Test]
        public void Should_throw_an_exception_when_trying_to_download_a_blob_with_a_null_name()
        {
            var blobStorageClient = CreateBlobStorageClient();

            Assert.Throws<ArgumentException>(() => blobStorageClient.DownloadBlob(null));
        }

        [Test]
        public void Should_throw_an_exception_when_trying_to_download_a_blob_that_does_not_exist()
        {
            var blobStorageClient = CreateBlobStorageClient();
            var blobName = Guid.NewGuid().ToString();

            Assert.Throws<InvalidOperationException>(() => blobStorageClient.DownloadBlob(blobName));
        }

        [Test]
        public void Should_confirm_that_blob_does_exists_when_it_does()
        {
            var blobStorageClient = CreateBlobStorageClient();
            var blobName = Guid.NewGuid().ToString();
            var uploadStream = new MemoryStream(GenerateRandomByteArray(BlobSize));

            blobStorageClient.UploadBlob(uploadStream, blobName);

            Assert.IsTrue(blobStorageClient.BlobExists(blobName));
        }

        [Test]
        public void Should_confirm_that_blob_does_not_exists_when_it_does_not()
        {
            var blobStorageClient = CreateBlobStorageClient();
            var blobName = Guid.NewGuid().ToString();

            Assert.IsFalse(blobStorageClient.BlobExists(blobName));
        }

        [Test]
        public void Should_upload_a_blob()
        {
            var blobStorageClient = CreateBlobStorageClient();
            var blobName = Guid.NewGuid().ToString();
            var uploadByteArray = GenerateRandomByteArray(BlobSize);
            var uploadStream = new MemoryStream(uploadByteArray);

            blobStorageClient.UploadBlob(uploadStream, blobName);

            var downloadStream = blobStorageClient.DownloadBlob(blobName);

            Assert.IsNotNull(downloadStream);

            using (var memoryStream = new MemoryStream())
            {
                downloadStream.CopyTo(memoryStream);

                var downloadByteArray = memoryStream.ToArray();

                Assert.IsTrue(uploadByteArray.SequenceEqual(downloadByteArray));
            }
        }

        [Test]
        public void Should_list_blobs()
        {
            var blobStorageClient = CreateBlobStorageClient();
            var uploadDictionary = new Dictionary<string, byte[]>();

            for (var i = 0; i < 5; i++)
            {
                uploadDictionary.Add(Guid.NewGuid().ToString(),
                                     GenerateRandomByteArray(BlobSize));
            }

            foreach (var uploadKey in uploadDictionary.Keys)
            {
                var uploadStream = new MemoryStream(uploadDictionary[uploadKey]);

                blobStorageClient.UploadBlob(uploadStream, uploadKey);
            }

            var blobList = blobStorageClient.ListBlobs().ToList();

            Assert.IsNotNull(blobList);
            Assert.IsTrue(uploadDictionary.Keys.Order().SequenceEqual(blobList.Order()));
        }

        [Test]
        public void Should_throw_an_exception_when_trying_to_save_a_null_blob()
        {
            var blobStorageClient = CreateBlobStorageClient();
            var blobName = Guid.NewGuid().ToString();
            
            Assert.Throws<ArgumentNullException>(() => blobStorageClient.UploadBlob(null, blobName));
        }

        [Test]
        public void Should_throw_an_exception_when_trying_to_save_an_empty_blob()
        {
            var blobStorageClient = CreateBlobStorageClient();
            var blobName = Guid.NewGuid().ToString();

            Assert.Throws<ArgumentException>(() => blobStorageClient.UploadBlob(new MemoryStream(), blobName));
        }

        [Test]
        public void Should_throw_an_exception_when_trying_to_save_a_blob_with_an_empty_name()
        {
            var blobStorageClient = CreateBlobStorageClient();
            var uploadStream = new MemoryStream(GenerateRandomByteArray(BlobSize));

            Assert.Throws<ArgumentException>(() => blobStorageClient.UploadBlob(uploadStream, string.Empty));
        }

        [Test]
        public void Should_throw_an_exception_when_trying_to_save_a_blob_with_a_null_name()
        {
            var blobStorageClient = CreateBlobStorageClient();
            var uploadStream = new MemoryStream(GenerateRandomByteArray(BlobSize));

            Assert.Throws<ArgumentException>(() => blobStorageClient.UploadBlob(uploadStream, null));
        }

        [Test]
        public void Should_upload_multiple_blobs()
        {
            var blobStorageClient = CreateBlobStorageClient();
            var uploadDictionary = new Dictionary<string, byte[]>();

            for (var i = 0; i < 5; i++)
            {
                uploadDictionary.Add(Guid.NewGuid().ToString(),
                                     GenerateRandomByteArray(BlobSize));
            }

            foreach (var uploadKey in uploadDictionary.Keys)
            {
                var uploadStream = new MemoryStream(uploadDictionary[uploadKey]);

                blobStorageClient.UploadBlob(uploadStream, uploadKey);
            }

            foreach (var uploadKey in uploadDictionary.Keys)
            {
                var downloadStream = blobStorageClient.DownloadBlob(uploadKey);

                Assert.IsNotNull(downloadStream);

                using (var memoryStream = new MemoryStream())
                {
                    downloadStream.CopyTo(memoryStream);

                    var downloadByteArray = memoryStream.ToArray();

                    Assert.IsTrue(uploadDictionary[uploadKey].SequenceEqual(downloadByteArray));
                }
            }
        }
    }
}