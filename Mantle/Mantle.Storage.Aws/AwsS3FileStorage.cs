using System;
using System.IO;
using System.Linq;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Mantle.Aws;

namespace Mantle.Storage.Aws
{
    public class AwsS3FileStorage : IFileStorage
    {
        private readonly IAwsConfiguration awsConfiguration;

        public AwsS3FileStorage(IAwsConfiguration awsConfiguration)
        {
            this.awsConfiguration = awsConfiguration;
        }

        public string BucketName { get; set; }

        public bool Exists(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name is required.", "fileName");

            ValidateBucketName();

            using (AmazonS3 client = CreateS3Client())
            {
                if (DoesBucketExist(client) == false)
                    return false;

                return DoesObjectExist(client, fileName);
            }
        }

        public Stream Load(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name is required.", "fileName");

            ValidateBucketName();

            using (AmazonS3 client = CreateS3Client())
            {
                if (DoesBucketExist(client) == false)
                    throw new InvalidOperationException(
                        String.Format("AWS S3 bucket [{0}] does not exist. File not found.", BucketName));

                if (DoesObjectExist(client, fileName))
                    throw new InvalidOperationException(
                        String.Format("AWS S3 object [{0}/{1}] does not exist. File not found.", BucketName, fileName));

                GetObjectRequest objectRequest = new GetObjectRequest().WithBucketName(BucketName).WithKey(fileName);

                using (GetObjectResponse objectResponse = client.GetObject(objectRequest))
                    return objectResponse.ResponseStream;
            }
        }

        public void Save(Stream fileContents, string fileName)
        {
            if (fileContents == null)
                throw new ArgumentNullException("fileContents");

            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name is required.", "fileName");

            using (AmazonS3 client = CreateS3Client())
            {
                if (DoesBucketExist(client) == false)
                    SetupBucket(client);

                PutObjectRequest objectRequest =
                    new PutObjectRequest().WithBucketName(BucketName).WithKey(fileName);

                objectRequest.InputStream = fileContents;

                client.PutObject(objectRequest);
            }
        }

        private bool DoesObjectExist(AmazonS3 client, string objectName)
        {
            using (
                ListObjectsResponse listObjectsResponse =
                    client.ListObjects(new ListObjectsRequest {BucketName = BucketName}))
                return listObjectsResponse.S3Objects.Any(o => (o.Key == objectName));
        }

        private bool DoesBucketExist(AmazonS3 client)
        {
            using (ListBucketsResponse listBucketsResponse = client.ListBuckets())
                return (listBucketsResponse.Buckets.Any(b => b.BucketName == BucketName));
        }

        private void SetupBucket(AmazonS3 client)
        {
            client.PutBucket(new PutBucketRequest {BucketName = BucketName});
        }

        private void ValidateBucketName()
        {
            if (String.IsNullOrEmpty(BucketName))
                throw new InvalidOperationException("Bucket name not provided.");
        }

        public AmazonS3 CreateS3Client()
        {
            return AWSClientFactory.CreateAmazonS3Client(awsConfiguration.AccessKey, awsConfiguration.SecretKey);
        }
    }
}