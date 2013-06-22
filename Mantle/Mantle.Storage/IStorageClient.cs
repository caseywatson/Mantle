using System.IO;

namespace Mantle.Storage
{
    public interface IStorageClient
    {
        string Name { get; }

        bool DoesObjectExist(string objName);
        string[] ListObjects();
        Stream LoadObject(string objName);
        void SaveObject(Stream obj, string objName);
        void Validate();
    }
}