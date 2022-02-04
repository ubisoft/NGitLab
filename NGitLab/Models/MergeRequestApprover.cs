using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestApprover
    {
        [DataMember(Name = "user")]
        [JsonPropertyName("user")]
        public User User;
    }
}
