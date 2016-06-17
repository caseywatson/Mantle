using Mantle.Interfaces;
using Mantle.Providers;
using Mantle.Serializers;
using Ninject.Modules;

namespace Mantle.PhotoGallery.Processor.Console.Mantle.Default
{
    public class MantleModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof(IDirectory<>))
                .To(typeof(DefaultDirectory<>))
                .InTransientScope();

            Bind(typeof(ISerializer<>))
                .To(typeof(JsonSerializer<>))
                .InTransientScope();

            Bind(typeof(ITypeMetadata<>))
                .To(typeof(TypeMetadata<>))
                .InSingletonScope();

            Bind<ITypeTokenProvider>()
                .To<AssemblyQualifiedNameTypeTokenProvider>()
                .InTransientScope();

            Bind<ITypeTokenProvider>()
                .To<DataContractTypeTokenProvider>()
                .InTransientScope();

            Bind<ITypeTokenProvider>()
                .To<FullNameTypeTokenProvider>()
                .InTransientScope();

            Bind<ITypeTokenProvider>()
                .To<NameTypeTokenProvider>()
                .InTransientScope();
        }
    }
}