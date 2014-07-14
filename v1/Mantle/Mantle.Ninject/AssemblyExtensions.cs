using System.Collections.Generic;
using System.Reflection;
using Mantle.Extensions;
using Ninject.Modules;

namespace Mantle.Ninject
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<INinjectModule> LoadProfileNinjectModules(this Assembly sourceAssembly,
                                                                            params string[] profileNames)
        {
            sourceAssembly.Require("sourceAssembly");
            return sourceAssembly.LoadAllFromProfile<INinjectModule>(profileNames);
        }
    }
}