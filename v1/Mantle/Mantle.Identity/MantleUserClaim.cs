using System;
using Mantle.Extensions;

namespace Mantle.Identity
{
    public class MantleUserClaim
    {
        public MantleUserClaim()
        {
            Id = Guid.NewGuid().ToString();
        }

        public MantleUserClaim(string userId, string claimType, string claimValue)
        {
            userId.Require(nameof(userId));
            claimType.Require(nameof(claimType));
            claimValue.Require(nameof(claimValue));

            UserId = userId;
            ClaimType = claimType;
            ClaimValue = claimValue;
        }

        public virtual string Id { get; set; }
        public virtual string UserId { get; set; }
        public virtual string ClaimType { get; set; }
        public virtual string ClaimValue { get; set; }
    }
}