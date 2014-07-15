using Mantle.Interfaces;
using Mantle.Providers;
using Mantle.Serializers;
using Ninject.Modules;

namespace Mantle.Sample.PublisherConsole.Mantle.Profiles.Default
{
    public class MantleModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof (IDirectory<>)).To(typeof (DefaultDirectory<>)).InTransientScope();

            Bind(typeof (ISerializer<>)).To(typeof (JsonSerializer<>)).InTransientScope();

            Bind<ITypeTokenProvider>().To<AssemblyQualifiedNameTypeTokenProvider>().InTransientScope();
            Bind<ITypeTokenProvider>().To<DataContractTypeTokenProvider>().InTransientScope();
            Bind<ITypeTokenProvider>().To<FullNameTypeTokenProvider>().InTransientScope();
            Bind<ITypeTokenProvider>().To<NameTypeTokenProvider>().InTransientScope();
        }
    }
}