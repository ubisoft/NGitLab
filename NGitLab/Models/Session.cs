using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Session : User
    {
        public new const string Url = "/session";

        [JsonPropertyName("private_token")]
        public string PrivateToken;
    }
}
