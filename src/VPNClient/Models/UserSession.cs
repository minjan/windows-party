using Newtonsoft.Json;

namespace VPNClient.Models
{
    public class UserSession
    {
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
    }
}
