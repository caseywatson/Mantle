using Mantle.Storage;
using Mantle.Storage.FileSystem;
using Ninject.Modules;

namespace Mantle.Sample.AddressBook.Storage.ConsoleHost.Mantle
{
    public class FileSystemStorageModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IStorageClient>()
                .To<FileSystemStorageClient>()
                .InSingletonScope()
                .OnActivation(c => c.Setup("PersonStorage", "C:\\People\\"));
        }
    }
}