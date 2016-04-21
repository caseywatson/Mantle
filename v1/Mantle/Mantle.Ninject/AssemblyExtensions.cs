using System.Collections.Generic;
using System.Reflection;
using Mantle.Extensions;
using Ninject.Modules;

namespace Mantle.Ninject
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<INinjectModule> LoadConfiguredProfileMantleModules(this Assembly sourceAssembly,
            string appSettingName =
                "MantleProfiles")
        {
            sourceAssembly.Require("sourceAssembly");
            appSettingName.Require("appSettingName");

            return sourceAssembly.LoadAllFromConfiguredProfiles<INinjectModule>();
        }

        public static IEnumerable<INinjectModule> LoadProfileNinjectModules(this Assembly sourceAssembly,
            params string[] profileNames)
        {
            sourceAssembly.Require("sourceAssembly");
            profileNames.Require("profileNames");

            return sourceAssembly.LoadAllFromProfiles<INinjectModule>(profileNames);
        }
    }
}