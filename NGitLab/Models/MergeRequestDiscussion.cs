using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestDiscussion
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("individual_note")]
        public bool IndividualNote { get; set; }

        [JsonPropertyName("notes")]
        public MergeRequestComment[] Notes { get; set; }
    }
}
