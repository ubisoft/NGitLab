using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class PersonInfo
    {
        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name;

        [DataMember(Name = "email")]
        [JsonPropertyName("email")]
        public string Email;
    }
}
