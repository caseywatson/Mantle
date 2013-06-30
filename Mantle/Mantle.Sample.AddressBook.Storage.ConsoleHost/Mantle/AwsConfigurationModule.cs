using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mantle.Aws;
using Ninject.Modules;

namespace Mantle.Sample.AddressBook.Storage.ConsoleHost.Mantle
{
    public class AwsConfigurationModule : NinjectModule
    {
        public override void Load()
        {
            // TODO: Setup your AWS configuration.

            Bind<IAwsConfiguration>()
                .To<AwsConfiguration>()
                .InSingletonScope()
                .OnActivation(c => c.Setup(
                    "AKIAJHUQPFSNH37KQDHA",
                    "D0vWHUzUVnpYuN97lF4Erpu0+1kThwBwHjZ7Qeyb"));
        }
    }
}
