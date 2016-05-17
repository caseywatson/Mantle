using System;
using Mantle.Extensions;

namespace Mantle.Identity
{
    public class MantleUserLogin
    {
        public MantleUserLogin()
        {
            Id = Guid.NewGuid().ToString();
        }

        public MantleUserLogin(string userId, string loginProvider, string providerKey)
            : this()
        {
            userId.Require(nameof(userId));
            loginProvider.Require(nameof(loginProvider));
            providerKey.Require(nameof(providerKey));

            UserId = userId;
            LoginProvider = loginProvider;
            ProviderKey = providerKey;
        }

        public virtual string Id { get; set; }
        public virtual string UserId { get; set; }
        public virtual string LoginProvider { get; set; }
        public virtual string ProviderKey { get; set; }
    }
}