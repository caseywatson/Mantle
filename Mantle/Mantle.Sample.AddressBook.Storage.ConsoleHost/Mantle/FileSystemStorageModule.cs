using Mantle.Storage;
using Mantle.Storage.FileSystem;
using Ninject.Modules;

namespace Mantle.Sample.AddressBook.Storage.ConsoleHost.Mantle
{
    public class FileSystemStorageModule : NinjectModule
    {
        public override void Load()
        {
            // TODO: Setup file system storage clients here.

            //Bind<IStorageClient>()
            //    .To<FileSystemStorageClient>()
            //    .InSingletonScope()
            //    .OnActivation(c => c.Configure("My Storage Client", "Replace this text with your storage directory path."));
        }
    }
}