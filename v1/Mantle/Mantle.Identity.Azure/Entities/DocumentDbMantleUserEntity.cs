using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mantle.Identity.Azure.Entities
{
    public class DocumentDbMantleUserEntity
    {
        [JsonProperty(PropertyName = "id")]
        public virtual string Id { get; set; } = Guid.NewGuid().ToString();

        public virtual bool EmailConfirmed { get; set; }
        public virtual bool LockoutEnabled { get; set; }
        public virtual bool PhoneNumberConfirmed { get; set; }
        public virtual bool TwoFactorEnabled { get; set; }

        public virtual DateTimeOffset LockoutEndDate { get; set; }

        public virtual int AccessFailedCount { get; set; }

        public virtual List<MantleUserClaim> Claims { get; set; } = new List<MantleUserClaim>();
        public virtual List<MantleUserLogin> Logins { get; set; } = new List<MantleUserLogin>();
        public virtual List<string> Roles { get; set; } = new List<string>();

        public virtual string Email { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string SecurityStamp { get; set; }
        public virtual string UserName { get; set; }
    }
}