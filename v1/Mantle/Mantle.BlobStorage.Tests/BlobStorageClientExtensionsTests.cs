using System;
using System.IO;
using System.Linq;
using System.Text;
using Mantle.BlobStorage.Extensions;
using Mantle.BlobStorage.Interfaces;
using Moq;
using NUnit.Framework;

namespace Mantle.BlobStorage.Tests
{
    [TestFixture]
    public class BlobStorageClientExtensionsTests
    {
        public const string BlobName = "TestBlob";

        [Test]
        public void Should_download_bytes()
        {
            var testByteArray = new byte[] {1, 2, 3, 4, 5};
            var testStream = new MemoryStream(testByteArray) {Position = 0};
            var mockBlobStorageClient = new Mock<IBlobStorageClient>();

            mockBlobStorageClient.Setup(c => c.DownloadBlob(BlobName)).Returns(testStream);

            var bytes = mockBlobStorageClient.Object.DownloadBytes(BlobName);

            Assert.IsNotNull(bytes);
            Assert.IsTrue(testByteArray.SequenceEqual(bytes));
        }

        [Test]
        public void Should_download_text()
        {
            const string testText = "This is a test.";

            var testStream = new MemoryStream(Encoding.UTF8.GetBytes(testText)) {Position = 0};
            var mockBlobStorageClient = new Mock<IBlobStorageClient>();

            mockBlobStorageClient.Setup(c => c.DownloadBlob(BlobName)).Returns(testStream);

            var text = mockBlobStorageClient.Object.DownloadText(BlobName);

            Assert.IsNotNull(text);
            Assert.AreEqual(text, testText);
        }

        [Test]
        public void Should_upload_bytes()
        {
            var testByteArray = new byte[] {1, 2, 3, 4, 5};
            var mockBlobStorageClient = new Mock<IBlobStorageClient>();

            mockBlobStorageClient.Object.UploadBytes(testByteArray, BlobName);

            mockBlobStorageClient.Verify(
                c =>
                    c.UploadBlob(
                        It.Is<MemoryStream>(
                            s =>
                                (s.ToArray()
                                .SequenceEqual(testByteArray))),
                        BlobName));
        }

        [Test]
        public void Should_upload_text()
        {
            const string testText = "Test";

            var testTextBytes = Encoding.UTF8.GetBytes(testText);
            var mockBlobStorageClient = new Mock<IBlobStorageClient>();

            mockBlobStorageClient.Object.UploadText(testText, BlobName);

            mockBlobStorageClient.Verify(
                c =>
                    c.UploadBlob(
                        It.Is<MemoryStream>(
                            s =>
                                (s.ToArray()
                                .SequenceEqual(testTextBytes))),
                        BlobName));
        }

        [Test]
        public void When_downloading_bytes_should_throw_an_ArgumentException_if_blob_name_is_null()
        {
            var mockBlobStorageClient = new Mock<IBlobStorageClient>();
            var ex = Assert.Throws<ArgumentException>(() => mockBlobStorageClient.Object.DownloadBytes(null));

            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.ParamName, "blobName");
        }

        [Test]
        public void When_downloading_bytes_should_throw_an_ArgumentNullException_if_IBlobStorageClient_is_null()
        {
            IBlobStorageClient blobStorageClient = null;
            var ex = Assert.Throws<ArgumentNullException>(() => blobStorageClient.DownloadBytes(BlobName));

            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.ParamName, "blobStorageClient");
        }

        [Test]
        public void When_downloading_file_should_throw_an_ArgumentException_if_blob_name_is_null()
        {
            var mockBlobStorageClient = new Mock<IBlobStorageClient>();
            var ex =
                Assert.Throws<ArgumentException>(() => mockBlobStorageClient.Object.DownloadFile("SomeFile.txt", null));

            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.ParamName, "blobName");
        }

        [Test]
        public void When_downloading_file_should_throw_an_ArgumentException_if_file_path_is_null()
        {
            var mockBlobStorageClient = new Mock<IBlobStorageClient>();
            var ex = Assert.Throws<ArgumentException>(() => mockBlobStorageClient.Object.DownloadFile(null, BlobName));

            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.ParamName, "filePath");
        }

        [Test]
        public void When_downloading_file_should_throw_an_ArgumentNullException_if_IBlobStorageClient_is_null()
        {
            IBlobStorageClient blobStorageClient = null;
            var ex = Assert.Throws<ArgumentNullException>(() => blobStorageClient.DownloadFile("SomeFile.txt", BlobName));

            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.ParamName, "blobStorageClient");
        }

        [Test]
        public void When_downloading_text_should_throw_an_ArgumentException_if_blob_name_is_null()
        {
            var mockBlobStorageClient = new Mock<IBlobStorageClient>();
            var ex = Assert.Throws<ArgumentException>(() => mockBlobStorageClient.Object.DownloadText(null));

            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.ParamName, "blobName");
        }

        [Test]
        public void When_downloading_text_should_throw_an_ArgumentNullException_if_IBlobStorageClient_is_null()
        {
            IBlobStorageClient blobStorageClient = null;
            var ex = Assert.Throws<ArgumentNullException>(() => blobStorageClient.DownloadText(BlobName));

            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.ParamName, "blobStorageClient");
        }

        [Test]
        public void When_uploading_bytes_should_throw_ArgumentException_if_blob_name_is_null()
        {
            var mockBlobStorageClient = new Mock<IBlobStorageClient>();
            var ex =
                Assert.Throws<ArgumentException>(
                    () => mockBlobStorageClient.Object.UploadBytes(new byte[] {0}, null));

            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.ParamName, "blobName");
        }

        [Test]
        public void When_uploading_bytes_should_throw_ArgumentException_if_bytes_are_empty()
        {
            var mockBlobStorageClient = new Mock<IBlobStorageClient>();
            var ex =
                Assert.Throws<ArgumentException>(() => mockBlobStorageClient.Object.UploadBytes(new byte[0], BlobName));

            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.ParamName, "bytes");
        }

        [Test]
        public void When_uploading_bytes_should_throw_ArgumentNullException_if_bytes_are_null()
        {
            var mockBlobStorageClient = new Mock<IBlobStorageClient>();
            var ex = Assert.Throws<ArgumentNullException>(() => mockBlobStorageClient.Object.UploadBytes(null, BlobName));

            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.ParamName, "bytes");
        }

        [Test]
        public void When_uploading_bytes_should_throw_ArgumentNullException_if_IBlobStorageClient_is_null()
        {
            IBlobStorageClient blobStorageClient = null;
            var ex = Assert.Throws<ArgumentNullException>(() => blobStorageClient.UploadBytes(new byte[] {0}, BlobName));

            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.ParamName, "blobStorageClient");
        }

        [Test]
        public void When_uploading_file_should_throw_ArgumentException_if_blob_name_is_null()
        {
            var mockBlobStorageClient = new Mock<IBlobStorageClient>();
            var ex =
                Assert.Throws<ArgumentException>(() => mockBlobStorageClient.Object.UploadFile("SomeFile.txt", null));

            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.ParamName, "blobName");
        }

        [Test]
        public void When_uploading_file_should_throw_ArgumentException_if_file_path_is_null()
        {
            var mockBlobStorageClient = new Mock<IBlobStorageClient>();
            var ex = Assert.Throws<ArgumentException>(() => mockBlobStorageClient.Object.UploadFile(null, BlobName));

            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.ParamName, "filePath");
        }

        [Test]
        public void When_uploading_file_should_throw_ArgumentNullException_if_IBlobStorageClient_is_null()
        {
            IBlobStorageClient blobStorageClient = null;
            var ex = Assert.Throws<ArgumentNullException>(() => blobStorageClient.UploadFile("SomeFile.txt", BlobName));

            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.ParamName, "blobStorageClient");
        }

        [Test]
        public void When_uploading_text_should_throw_ArgumentException_if_blob_name_is_null()
        {
            var mockBlobStorageClient = new Mock<IBlobStorageClient>();
            var ex = Assert.Throws<ArgumentException>(() => mockBlobStorageClient.Object.UploadText("Test", null));

            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.ParamName, "blobName");
        }

        [Test]
        public void When_uploading_text_should_throw_ArgumentException_if_text_is_null()
        {
            var mockBlobStorageClient = new Mock<IBlobStorageClient>();
            var ex = Assert.Throws<ArgumentException>(() => mockBlobStorageClient.Object.UploadText(null, BlobName));

            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.ParamName, "text");
        }

        [Test]
        public void When_uploading_text_should_throw_ArgumentNullException_if_IBlobStorageClient_is_null()
        {
            IBlobStorageClient blobStorageClient = null;
            var ex = Assert.Throws<ArgumentNullException>(() => blobStorageClient.UploadText("Test", BlobName));

            Assert.IsNotNull(ex);
            Assert.AreEqual(ex.ParamName, "blobStorageClient");
        }
    }
}