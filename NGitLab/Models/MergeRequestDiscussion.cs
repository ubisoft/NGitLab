using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestDiscussion
    {
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [DataMember(Name = "individual_note")]
        [JsonPropertyName("individual_note")]
        public bool IndividualNote { get; set; }

        [DataMember(Name = "notes")]
        [JsonPropertyName("notes")]
        public MergeRequestComment[] Notes { get; set; }
    }
}
