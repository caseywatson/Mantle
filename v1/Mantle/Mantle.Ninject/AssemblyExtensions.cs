using System.Collections.Generic;
using System.Reflection;
using Mantle.Extensions;
using Ninject.Modules;

namespace Mantle.Ninject
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<INinjectModule> LoadProfileNinjectModules(
            this Assembly sourceAssembly,
            params string[] profileNames)
        {
            sourceAssembly.Require(nameof(sourceAssembly));
            profileNames.Require(nameof(profileNames));

            return sourceAssembly.LoadAllFromProfiles<INinjectModule>(profileNames);
        }
    }
}