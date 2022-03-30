using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class BranchCreate
    {
        [JsonPropertyName("branch")]
        public string Name;

        [JsonPropertyName("ref")]
        public string Ref;
    }
}
