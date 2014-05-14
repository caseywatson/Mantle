using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Ninject.Modules;

namespace Mantle.Ninject
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<INinjectModule> GetConfiguredMantleModules(this Assembly sourceAssembly,
            string appSettingName = "MantleProfiles")
        {
            if (sourceAssembly == null)
                throw new ArgumentNullException("sourceAssembly");

            var appSettings = ConfigurationManager.AppSettings;

            if (appSettings[appSettingName] == null)
                throw new ConfigurationErrorsException(String.Format("Mantle profiles [{0}] not configured.",
                    appSettingName));

            var moduleNames =
                appSettings[appSettingName].Split(',', ';').Select(p => p.Trim()).Where(p => (p.Length > 0)).ToArray();

            if (moduleNames.Length == 0)
                throw new ConfigurationErrorsException(String.Format("Mantle profiles [{0}] not configured.",
                    appSettingName));

            return sourceAssembly.GetMantleModules(moduleNames);
        }

        public static IEnumerable<INinjectModule> GetMantleModules(this Assembly sourceAssembly,
            params string[] profileNames)
        {
            if (sourceAssembly == null)
                throw new ArgumentNullException("sourceAssembly");

            string rootNamespace = sourceAssembly.GetName().Name;
            var moduleNamespaces = new List<string>();

            moduleNamespaces.Add(String.Format("{0}.Mantle.Boilerplate", rootNamespace));

            if ((profileNames != null) && (profileNames.Length > 0))
                moduleNamespaces.AddRange(
                    profileNames.Select(p => String.Format("{0}.Mantle.Profiles.{1}", rootNamespace, p)));

            return
                sourceAssembly.GetExportedTypes()
                    .Where(t => (IsLoadableModule(t)) && (IsInAnyNamespace(t, moduleNamespaces)))
                    .Select(t => (Activator.CreateInstance(t) as INinjectModule));
        }

        private static bool IsLoadableModule(Type type)
        {
            return typeof (INinjectModule).IsAssignableFrom(type)
                   && !type.IsAbstract
                   && !type.IsInterface
                   && type.GetConstructor(Type.EmptyTypes) != null;
        }

        private static bool IsInAnyNamespace(Type type, IEnumerable<string> namespaces)
        {
            return (namespaces.Any(n => ((type.Namespace == n) || (type.Namespace.StartsWith(n + ".")))));
        }
    }
}