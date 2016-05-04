using System.Configuration;
using System.Linq;
using Mantle.Extensions;
using Mantle.Interfaces;

namespace Mantle.Providers
{
    public class AppSettingProfileNamesProvider : IProfileNamesProvider
    {
        private readonly string settingName;

        public AppSettingProfileNamesProvider(string settingName = "MantleProfiles")
        {
            settingName.Require(nameof(settingName));

            this.settingName = settingName;
        }

        public string[] GetProfileNames()
        {
            var appSettings = ConfigurationManager.AppSettings;

            if (appSettings[settingName] == null)
                throw new ConfigurationErrorsException($"Profiles [{settingName}] not configured.");

            var profileNames = appSettings[settingName].ParseProfileNames().ToArray();

            if (profileNames.None())
                throw new ConfigurationErrorsException($"Profiles [{settingName}] not configured.");

            return profileNames;
        }
    }
}