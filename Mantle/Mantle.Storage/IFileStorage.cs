using System.IO;

namespace Mantle.Storage
{
    public interface IFileStorage
    {
        bool Exists(string fileName);
        Stream Load(string fileName);
        void Save(Stream fileContents, string fileName);
    }
}