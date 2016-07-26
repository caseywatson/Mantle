using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Amazon.S3;
using Amazon.S3.Model;
using Mantle.Aws.Interfaces;
using Mantle.BlobStorage.Interfaces;
using Mantle.Configuration.Attributes;
using Mantle.Extensions;
using Mantle.FaultTolerance.Interfaces;

namespace Mantle.BlobStorage.Aws.Clients
{
    public class AwsS3BlobStorageClient : IBlobStorageClient, IDisposable
    {
        private readonly IAwsRegionEndpoints awsRegionEndpoints;
        private readonly ITransientFaultStrategy transientFaultStrategy;

        private AmazonS3Client amazonS3Client;

        public AwsS3BlobStorageClient(IAwsRegionEndpoints awsRegionEndpoints,
                                      ITransientFaultStrategy transientFaultStrategy)
        {
            this.awsRegionEndpoints = awsRegionEndpoints;
            this.transientFaultStrategy = transientFaultStrategy;
        }

        [Configurable]
        public bool AutoSetup { get; set; }

        [Configurable(IsRequired = true)]
        public string AwsAccessKeyId { get; set; }

        [Configurable(IsRequired = true)]
        public string AwsSecretAccessKey { get; set; }

        [Configurable(IsRequired = true)]
        public string AwsRegionName { get; set; }

        [Configurable(IsRequired = true)]
        public string BucketName { get; set; }

        public AmazonS3Client AmazonS3Client => GetAmazonS3Client();

        public bool BlobExists(string blobName)
        {
            blobName.Require(nameof(blobName));

            if (DoesBucketExist() == false)
                return false;

            return transientFaultStrategy.Try(
                () => AmazonS3Client.ListObjects(BucketName, blobName).S3Objects.Any(o => o.Key == blobName));
        }

        public void DeleteBlob(string blobName)
        {
            blobName.Require(nameof(blobName));

            if (DoesBucketExist() == false)
                throw new InvalidOperationException($"AWS S3 bucket [{BucketName}] does not exist.");

            if (BlobExists(blobName) == false)
                throw new InvalidOperationException($"AWS S3 object [{BucketName}/{blobName}] does not exist.");

            transientFaultStrategy.Try(() => AmazonS3Client.DeleteObject(BucketName, blobName));
        }

        public Stream DownloadBlob(string blobName)
        {
            blobName.Require(nameof(blobName));

            if (DoesBucketExist() == false)
                throw new InvalidOperationException($"AWS S3 bucket [{BucketName}] does not exist.");

            if (BlobExists(blobName) == false)
                throw new InvalidOperationException($"AWS S3 object [{BucketName}/{blobName}] does not exist.");

            var outputStream = new MemoryStream();
            var getObjectResponse = transientFaultStrategy.Try(() => AmazonS3Client.GetObject(BucketName, blobName));

            getObjectResponse.ResponseStream.CopyTo(outputStream);
            outputStream.TryToRewind();

            return outputStream;
        }

        public IEnumerable<string> ListBlobs()
        {
            if (DoesBucketExist() == false)
                throw new InvalidOperationException($"AWS S3 bucket [{BucketName}] does not exist.");

            return transientFaultStrategy.Try(() => AmazonS3Client.ListObjects(BucketName).S3Objects.Select(o => o.Key));
        }

        public void UploadBlob(Stream source, string blobName)
        {
            source.Require(nameof(source));
            blobName.Require(nameof(blobName));

            if (source.Length == 0)
                throw new ArgumentException($"[{nameof(source)}] is empty.", nameof(source));

            if (DoesBucketExist() == false)
                throw new InvalidOperationException($"AWS S3 bucket [{BucketName}] does not exist.");

            source.TryToRewind();

            var putObjectRequest = new PutObjectRequest
            {
                BucketName = BucketName,
                InputStream = source,
                Key = blobName
            };

            transientFaultStrategy.Try(() => AmazonS3Client.PutObject(putObjectRequest));
        }

        public void Dispose()
        {
            amazonS3Client?.Dispose();
        }

        private bool DoesBucketExist(AmazonS3Client amazonS3Client = null)
        {
            return transientFaultStrategy.Try(
                () => (amazonS3Client ?? AmazonS3Client).ListBuckets().Buckets.Any(b => b.BucketName == BucketName));
        }

        private AmazonS3Client GetAmazonS3Client()
        {
            if (amazonS3Client == null)
            {
                var regionEndpoint = awsRegionEndpoints.GetRegionEndpointByName(AwsRegionName);

                if (regionEndpoint == null)
                    throw new ConfigurationErrorsException($"[{AwsRegionName}] is not a known AWS region.");

                amazonS3Client = transientFaultStrategy.Try(
                    () => new AmazonS3Client(AwsAccessKeyId, AwsSecretAccessKey, regionEndpoint));

                if (AutoSetup && (DoesBucketExist(amazonS3Client) == false))
                    transientFaultStrategy.Try(() => amazonS3Client.PutBucket(BucketName));
            }

            return amazonS3Client;
        }
    }
}