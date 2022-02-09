using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class BranchCreate
    {
        [DataMember(Name = "branch")]
        [JsonPropertyName("branch")]
        public string Name;

        [DataMember(Name = "ref")]
        [JsonPropertyName("ref")]
        public string Ref;
    }
}
