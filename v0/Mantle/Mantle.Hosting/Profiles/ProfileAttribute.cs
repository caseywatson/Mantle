using System;

namespace Mantle.Hosting.Profiles
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class ProfileAttribute : Attribute
    {
        public ProfileAttribute(string profileName)
        {
            if (String.IsNullOrEmpty(profileName))
                throw new ArgumentNullException("profileName");

            ProfileName = profileName;
        }

        public string ProfileName { get; private set; }
    }
}