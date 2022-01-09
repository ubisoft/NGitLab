using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Session : User
    {
        public new const string Url = "/session";

        [DataMember(Name = "private_token")]
        [JsonPropertyName("private_token")]
        public string PrivateToken;
    }
}
