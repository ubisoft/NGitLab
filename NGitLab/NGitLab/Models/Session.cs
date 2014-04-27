using Newtonsoft.Json;

namespace NGitLab.Models
{
    public class Session : User
    {
        public new const string Url = "/session";

        [JsonProperty("private_token")]
        public string PrivateToken;
    }
}