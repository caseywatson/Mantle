using System;
using System.IO;

namespace Mantle.Storage.FileSystem
{
    public class FileSystemFileStorage : IFileStorage
    {
        public string DirectoryPath { get; set; }

        public bool Exists(string fileName)
        {
            ValidateDirectoryPath();

            return File.Exists(Path.Combine(DirectoryPath, fileName));
        }

        public Stream Load(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name is required.", "fileName");

            ValidateDirectoryPath();

            string filePath = Path.Combine(DirectoryPath, fileName);

            if (File.Exists(filePath) == false)
                throw new ArgumentException(String.Format("The requested file [{0}] was not found.", filePath),
                                            "fileName");

            return File.OpenRead(filePath);
        }

        public void Save(Stream fileContents, string fileName)
        {
            if (fileContents == null)
                throw new ArgumentNullException("fileContents");

            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name is required.", "fileName");

            ValidateDirectoryPath();
            SetupDirectory();

            using (FileStream fileStream = File.Create(Path.Combine(DirectoryPath, fileName)))
                fileContents.CopyTo(fileStream);
        }

        private void SetupDirectory()
        {
            if (Directory.Exists(DirectoryPath) == false)
                Directory.CreateDirectory(DirectoryPath);
        }

        private void ValidateDirectoryPath()
        {
            if (String.IsNullOrEmpty(DirectoryPath))
                throw new InvalidOperationException("Directory not provided.");
        }
    }
}