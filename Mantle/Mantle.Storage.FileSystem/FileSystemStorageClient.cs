using System;
using System.IO;
using System.Linq;

namespace Mantle.Storage.FileSystem
{
    public class FileSystemStorageClient : BaseStorageClient, IStorageClient
    {
        public string DirectoryPath { get; set; }

        public bool DoesObjectExist(string fileName)
        {
            ValidateFileName(fileName);

            try
            {
                return File.Exists(Path.Combine(DirectoryPath, fileName));
            }
            catch (Exception ex)
            {
                throw new StorageException("An error occurred while processing your request.", ex);
            }
        }

        public string[] ListObjects()
        {
            if (Directory.Exists(DirectoryPath) == false)
                throw new StorageException(String.Format("Directory [{0}] does not exist.", DirectoryPath));

            try
            {
                return new DirectoryInfo(DirectoryPath).GetFiles().Select(f => (f.Name)).ToArray();
            }
            catch (Exception ex)
            {
                throw new StorageException("An error occurred while processing your request.", ex);
            }
        }

        public Stream LoadObject(string fileName)
        {
            ValidateFileName(fileName);

            string filePath = Path.Combine(DirectoryPath, fileName);

            if (File.Exists(filePath) == false)
                throw new ArgumentException(String.Format("The requested file [{0}] was not found.", filePath));

            try
            {
                return File.OpenRead(filePath);
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

            ValidateFileName(fileName);

            try
            {
                SetupDirectory();

                using (FileStream fileStream = File.Create(Path.Combine(DirectoryPath, fileName)))
                    fileContents.CopyTo(fileStream);
            }
            catch (Exception ex)
            {
                throw new StorageException("An error occurred while processing your request.", ex);
            }
        }

        public override void Validate()
        {
            if (String.IsNullOrEmpty(DirectoryPath))
                throw new StorageException("Directory not provided.");

            try
            {
                var directoryInfo = new DirectoryInfo(DirectoryPath);
            }
            catch (Exception ex)
            {
                throw new StorageException(
                    String.Format("The provided directory path [{0}] is invalid.", DirectoryPath), ex);
            }
        }

        public void Configure(string name, string directoryPath)
        {
            Name = name;
            DirectoryPath = directoryPath;

            Validate();
        }

        private void SetupDirectory()
        {
            if (Directory.Exists(DirectoryPath) == false)
                Directory.CreateDirectory(DirectoryPath);
        }

        private void ValidateFileName(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name is required.", "fileName");

            try
            {
                var fileInfo = new FileInfo(fileName);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(String.Format("The provided file name [{0}] is invalid.", fileName), ex);
            }
        }
    }
}