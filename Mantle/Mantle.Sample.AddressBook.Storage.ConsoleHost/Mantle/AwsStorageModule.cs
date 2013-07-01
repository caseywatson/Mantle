using Mantle.Storage;
using Mantle.Storage.Aws;
using Ninject.Modules;

namespace Mantle.Sample.AddressBook.Storage.ConsoleHost.Mantle
{
    public class AwsStorageModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IStorageClient>()
                .To<AwsS3StorageClient>()
                .InSingletonScope()
                .OnActivation(c => c.Setup("PersonStorage", "Replace this text with the name of your AWS S3 bucket."));
        }
    }
}