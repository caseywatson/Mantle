using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace Mantle.Extensions
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<T> LoadAllFromConfiguredProfiles<T>(this Assembly sourceAssembly,
                                                                      string appSettingName = "MantleProfiles")
        {
            sourceAssembly.Require("sourceAssembly");
            appSettingName.Require("appSettingName");

            var appSettings = ConfigurationManager.AppSettings;

            if (appSettings[appSettingName] == null)
                throw new ConfigurationErrorsException(String.Format("Mantle profiles [{0}] not configured.",
                                                                     appSettingName));

            var profileNames =
                appSettings[appSettingName].Split(',', ';').Select(n => n.Trim()).Where(n => (n.Length > 0)).ToArray();

            if (profileNames.Length == 0)
                throw new ConfigurationErrorsException(String.Format("Mantle profiles [{0}] configuration invalid.",
                                                                     appSettingName));

            return sourceAssembly.LoadAllFromProfiles<T>(profileNames);
        }

        public static IEnumerable<T> LoadAllFromProfiles<T>(this Assembly sourceAssembly, params string[] profileNames)
        {
            sourceAssembly.Require("sourceAssembly");

            var rootNamespace = sourceAssembly.GetName().Name;
            var moduleNamespaces = new List<string>();

            if (profileNames.IsNullOrEmpty() == false)
                moduleNamespaces.AddRange(profileNames.Select(p => ToProfileNamespace(rootNamespace, p)));

            return sourceAssembly.GetExportedTypes()
                .Where(t => (IsLoadableAs<T>(t)) && (IsInAnyNamespace(t, moduleNamespaces)))
                .Select(t => ((T) (Activator.CreateInstance(t))));
        }

        private static bool IsInAnyNamespace(Type type, IEnumerable<string> namespaces)
        {
            if (type.Namespace == null)
                return false;

            return (namespaces.Any(n => ((type.Namespace == n) || (type.Namespace.StartsWith(n + '.')))));
        }

        private static bool IsLoadableAs<T>(Type type)
        {
            return (((typeof (T)).IsAssignableFrom(type))
                    && (type.IsAbstract == false)
                    && (type.IsInterface == false)
                    && (type.GetConstructor(Type.EmptyTypes) != null));
        }

        private static string ToProfileNamespace(string rootNamespace, string profileName)
        {
            return String.Format("{0}.Mantle.Profiles.{1}", rootNamespace, profileName);
        }
    }
}