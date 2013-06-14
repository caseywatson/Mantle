using System.IO;

namespace Mantle.Storage
{
    public interface IFileStorage
    {
        bool DoesFileExist(string fileName);
        string[] ListFiles();
        Stream LoadFile(string fileName);
        void SaveFile(Stream fileContents, string fileName);
    }
}