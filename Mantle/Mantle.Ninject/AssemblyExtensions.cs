using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ninject.Modules;

namespace Mantle.Ninject
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<INinjectModule> GetModulesByProfile(Assembly sourceAssembly, string profileName)
        {
            if (sourceAssembly == null)
                throw new ArgumentNullException("sourceAssembly");

            if (String.IsNullOrEmpty(profileName) == false)
                throw new ArgumentNullException("profileName");

            return sourceAssembly
                .GetExportedTypes()
                .Where(t => (IsLoadableModule(t) && IsInProfile(t, profileName)))
                .Select(t => (Activator.CreateInstance(t) as INinjectModule));
        }

        private static bool IsLoadableModule(Type type)
        {
            return typeof (INinjectModule).IsAssignableFrom(type)
                   && !type.IsAbstract
                   && !type.IsInterface
                   && type.GetConstructor(Type.EmptyTypes) != null;
        }

        private static bool IsInProfile(Type type, string profileName)
        {
            return
                type.GetCustomAttributes(typeof (ProfileAttribute), false)
                    .OfType<ProfileAttribute>()
                    .Any(pa => (pa.ProfileName == profileName));
        }
    }
}