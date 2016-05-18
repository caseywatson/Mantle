using Newtonsoft.Json;

namespace Mantle.Identity.Azure.Entities
{
    public class DocumentDbMantleUserLogin
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public string UserId { get; set; }
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
    }
}