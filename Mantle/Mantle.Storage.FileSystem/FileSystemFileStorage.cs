using System;
using System.IO;

namespace Mantle.Storage.FileSystem
{
    public class FileSystemFileStorage : IFileStorage
    {
        public string RootPath { get; set; }

        public bool Exists(string fileName)
        {
            return File.Exists(BuildPath(fileName));
        }

        public Stream Load(string fileName)
        {
            string filePath = BuildPath(fileName);

            if (File.Exists(filePath) == false)
                throw new ArgumentException(String.Format("The requested file [{0}] was not found.", filePath),
                                            "fileName");

            return File.OpenRead(filePath);
        }

        public void Save(Stream fileContents, string fileName)
        {
            if (fileContents == null)
                throw new ArgumentNullException("fileContents");

            using (FileStream fileStream = File.Create(BuildPath(fileName)))
                fileContents.CopyTo(fileStream);
        }

        private string BuildPath(string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name is required.", "fileName");

            if (RootPath != null)
                fileName = String.Format("{0}\\{1}", RootPath.TrimEnd('\\'), RootPath.TrimStart('\\'));

            try
            {
                var fileInfo = new FileInfo(fileName);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    String.Format("The file system path [{0}] is invalid. See inner exception.", fileName), "fileName",
                    ex);
            }

            return fileName;
        }
    }
}