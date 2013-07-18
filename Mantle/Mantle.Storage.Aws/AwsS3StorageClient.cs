using System;
using System.IO;
using System.Linq;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Mantle.Aws;

namespace Mantle.Storage.Aws
{
    public class AwsS3StorageClient : BaseStorageClient, IStorageClient
    {
        private readonly IAwsConfiguration awsConfiguration;

        public AwsS3StorageClient(IAwsConfiguration awsConfiguration)
        {
            awsConfiguration.Validate();

            this.awsConfiguration = awsConfiguration;
        }

        public string BucketName { get; set; }

        public bool DoesObjectExist(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name is required.", "fileName");

            try
            {
                using (AmazonS3 client = CreateS3Client())
                {
                    if (DoesBucketExist(client) == false)
                        return false;

                    return DoesObjectExist(client, fileName);
                }
            }
            catch (AmazonS3Exception s3Ex)
            {
                throw AnAwsRelatedException(s3Ex);
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
                using (AmazonS3 client = CreateS3Client())
                {
                    if (DoesBucketExist(client) == false)
                        throw new StorageException(String.Format("AWS S3 bucket [{0}] does not exist.", BucketName));

                    using (ListObjectsResponse listObjectsResponse =
                        client.ListObjects(new ListObjectsRequest {BucketName = BucketName}))
                        return listObjectsResponse.S3Objects.Select(o => (o.Key)).ToArray();
                }
            }
            catch (AmazonS3Exception s3Ex)
            {
                throw AnAwsRelatedException(s3Ex);
            }
            catch (Exception ex)
            {
                throw new StorageException("An error occurred while processing your request.", ex);
            }
        }

        public Stream LoadObject(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name is required.", "fileName");

            try
            {
                using (AmazonS3 client = CreateS3Client())
                {
                    if (DoesBucketExist(client) == false)
                        throw new StorageException(String.Format("AWS S3 bucket [{0}] does not exist. File not found.",
                            BucketName));

                    if (DoesObjectExist(client, fileName) == false)
                        throw new StorageException(
                            String.Format("AWS S3 object [{0}/{1}] does not exist. File not found.", BucketName,
                                fileName));

                    GetObjectRequest objectRequest = new GetObjectRequest().WithBucketName(BucketName).WithKey(fileName);

                    using (GetObjectResponse objectResponse = client.GetObject(objectRequest))
                    {
                        if (objectResponse.ResponseStream.CanSeek)
                            objectResponse.ResponseStream.Position = 0;

                        return objectResponse.ResponseStream;
                    }
                }
            }
            catch (AmazonS3Exception s3Ex)
            {
                throw AnAwsRelatedException(s3Ex);
            }
            catch (Exception ex)
            {
                throw new StorageException("An error occurred while processing your request.", ex);
            }
        }

        public void SaveObject(Stream fileContents, string fileName)
        {
            if (fileContents == null)
                throw new ArgumentNullException("fileContents");

            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name is required.", "fileName");

            try
            {
                using (AmazonS3 client = CreateS3Client())
                {
                    if (DoesBucketExist(client) == false)
                        SetupBucket(client);

                    PutObjectRequest objectRequest = new PutObjectRequest().WithBucketName(BucketName).WithKey(fileName);

                    if (fileContents.CanSeek)
                        fileContents.Position = 0;

                    objectRequest.InputStream = fileContents;

                    client.PutObject(objectRequest);
                }
            }
            catch (AmazonS3Exception s3Ex)
            {
                throw AnAwsRelatedException(s3Ex);
            }
            catch (Exception ex)
            {
                throw new StorageException("An error occurred while processing your request.", ex);
            }
        }

        public override void Validate()
        {
            base.Validate();

            if (String.IsNullOrEmpty(BucketName))
                throw new StorageException("Bucket name not provided.");

            if (BucketName.Where(Char.IsLetter).All(Char.IsLower) == false)
                throw new StorageException("Bucket name must contain only lower-case letters.");

            if (BucketName.All(c => (Char.IsLetterOrDigit(c)) || (c == '-') || (c == '.')) == false)
                throw new StorageException(
                    "Bucket name must contain only letters (A-Z), digits (0-9), periods (.) and hyphens (-).");

            if (Char.IsLetterOrDigit(BucketName[0]) == false)
                throw new StorageException("Bucket name must start with either a letter (A-Z) or digit (0-9).");

            if (Char.IsLetterOrDigit(BucketName.Last()) == false)
                throw new StorageException("Bucket name must end with either a letter (A-Z) or digit (0-9).");

            if (BucketName.Contains(".."))
                throw new StorageException("Bucket name must not contain repeating periods (..).");

            if (BucketName.Contains("--"))
                throw new StorageException("Bucket name must not contain repeating hyphens (--).");

            if (BucketName.Contains("-.") || BucketName.Contains(".-"))
                throw new StorageException("Bucket name must not contain (-.) or (.-).");

            if ((BucketName.Length < 3) || (BucketName.Length > 63))
                throw new StorageException("Bucket name must be between 3 and 63 characters in length.");
        }

        public void Configure(string name, string bucketName)
        {
            Name = name;
            BucketName = bucketName;

            Validate();
        }

        private bool DoesObjectExist(AmazonS3 client, string objectName)
        {
            using (ListObjectsResponse listObjectsResponse =
                client.ListObjects(new ListObjectsRequest {BucketName = BucketName}))
                return listObjectsResponse.S3Objects.Any(o => (o.Key == objectName));
        }

        private bool DoesBucketExist(AmazonS3 client)
        {
            using (ListBucketsResponse listBucketsResponse = client.ListBuckets())
                return (listBucketsResponse.Buckets.Any(b => b.BucketName == BucketName));
        }

        private StorageException AnAwsRelatedException(AmazonS3Exception ex)
        {
            if ((ex.ErrorCode != null) &&
                ((ex.ErrorCode.Equals("InvalidAccessKeyId")) || (ex.ErrorCode.Equals("InvalidSecurity"))))
                return new StorageException("The provided AWS credentials are invalid.", ex);

            return new StorageException("An AWS service related error occurred while processing your request.", ex);
        }

        private void SetupBucket(AmazonS3 client)
        {
            client.PutBucket(new PutBucketRequest {BucketName = BucketName});
        }

        public AmazonS3 CreateS3Client()
        {
            return AWSClientFactory.CreateAmazonS3Client(awsConfiguration.AccessKey, awsConfiguration.SecretKey);
        }
    }
}