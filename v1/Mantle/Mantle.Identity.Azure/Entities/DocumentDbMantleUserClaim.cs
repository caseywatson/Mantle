using Newtonsoft.Json;

namespace Mantle.Identity.Azure.Entities
{
    public class DocumentDbMantleUserClaim
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public string UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}