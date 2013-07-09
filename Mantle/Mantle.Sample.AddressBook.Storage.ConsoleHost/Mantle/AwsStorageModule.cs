using Mantle.Storage;
using Mantle.Storage.Aws;
using Ninject.Modules;

namespace Mantle.Sample.AddressBook.Storage.ConsoleHost.Mantle
{
    public class AwsStorageModule : NinjectModule
    {
        public override void Load()
        {
            // TODO: Setup AWS S3 storage clients here.

            //Bind<IStorageClient>()
            //    .To<AwsS3StorageClient>()
            //    .InSingletonScope()
            //    .OnActivation(c => c.Configure("My Storage Client", "Replace this text with the name of your AWS S3 bucket."));
        }
    }
}