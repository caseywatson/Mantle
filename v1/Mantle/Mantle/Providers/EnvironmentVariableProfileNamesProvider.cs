using System;
using System.Collections;
using System.Configuration;
using System.Linq;
using Mantle.Extensions;
using Mantle.Interfaces;

namespace Mantle.Providers
{
    public class EnvironmentVariableProfileNamesProvider : IProfileNamesProvider
    {
        private readonly string variableName;

        public EnvironmentVariableProfileNamesProvider(string variableName = "MantleProfiles")
        {
            variableName.Require(nameof(variableName));

            this.variableName = variableName;
        }

        public string[] GetProfileNames()
        {
            var variable = Environment.GetEnvironmentVariables()
                .OfType<DictionaryEntry>()
                .SingleOrDefault(e => (string) e.Key == variableName);

            if (variable.Value == null)
                throw new ConfigurationErrorsException($"Profiles [{variableName}] not configured.");

            var profileNames =
                variable.Value.ToString().Split(',', ';').Select(n => n.Trim()).Where(n => n.Length > 0).ToArray();

            if (profileNames.None())
                throw new ConfigurationErrorsException($"Profiles [{variableName}] not configured.");

            return profileNames;
        }
    }
}