using System.Configuration;
using System.Linq;
using Mantle.Extensions;
using Mantle.Interfaces;

namespace Mantle.Providers
{
    public class AppSettingProfileProvider : IProfileNamesProvider
    {
        private readonly string settingName;

        public AppSettingProfileProvider(string settingName = "MantleProfiles")
        {
            settingName.Require(nameof(settingName));

            this.settingName = settingName;
        }

        public string[] GetProfiles()
        {
            var appSettings = ConfigurationManager.AppSettings;

            if (appSettings[settingName] == null)
                throw new ConfigurationErrorsException($"Profiles [{settingName}] not configured.");

            var profiles = appSettings[settingName].ParseProfileNames().ToArray();

            if (profiles.None())
                throw new ConfigurationErrorsException($"Profiles [{settingName}] not configured.");

            return profiles;
        }
    }
}