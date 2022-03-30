using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestApprover
    {
        [JsonPropertyName("user")]
        public User User;
    }
}
