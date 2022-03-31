using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class BranchCreate
    {
        [JsonPropertyName("branch")]
        public string Name;

        [JsonPropertyName("ref")]
        public string Ref;
    }
}
