using System;
using System.Collections.Generic;
using Ninject;

namespace Mantle.Configuration.Ninject
{
// ReSharper disable once InconsistentNaming
    public static class INinjectModuleExtensions
    {
        public static void LoadFrom<TService, TConcrete>(this IKernel kernel,
                                                         IEnumerable<IConfigurationMetadata> metadataCollection,
                                                         bool inSingletonScope = false)
            where TConcrete : TService, IConfigurable
        {
            if (kernel == null)
                throw new ArgumentNullException("kernel");

            if (metadataCollection == null)
                throw new ArgumentNullException("metadataCollection");

            foreach (IConfigurationMetadata metadata in metadataCollection)
            {
                IConfigurationMetadata m = metadata;

                if (inSingletonScope)
                    kernel.Bind<TService>().To<TConcrete>().InSingletonScope().OnActivation(c => c.Configure(m));
                else
                    kernel.Bind<TService>().To<TConcrete>().InTransientScope().OnActivation(c => c.Configure(m));
            }
        }
    }
}