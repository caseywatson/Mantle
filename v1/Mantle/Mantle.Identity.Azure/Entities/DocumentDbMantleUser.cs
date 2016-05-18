using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mantle.Identity.Azure.Entities
{
    public class DocumentDbMantleUser
    {
        [JsonProperty(PropertyName = "id")]
        public virtual string Id { get; set; } = Guid.NewGuid().ToString();

        public virtual bool EmailConfirmed { get; set; }
        public virtual bool LockoutEnabled { get; set; }
        public virtual bool PhoneNumberConfirmed { get; set; }
        public virtual bool TwoFactorEnabled { get; set; }

        public virtual DateTimeOffset LockoutEndDate { get; set; }

        public virtual int AccessFailedCount { get; set; }

        public virtual List<DocumentDbMantleUserClaim> Claims { get; set; } = new List<DocumentDbMantleUserClaim>();
        public virtual List<DocumentDbMantleUserLogin> Logins { get; set; } = new List<DocumentDbMantleUserLogin>();
        public virtual List<string> Roles { get; set; } = new List<string>();

        public virtual string Email { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string SecurityStamp { get; set; }
        public virtual string UserName { get; set; }
    }
}