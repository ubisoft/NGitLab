using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Change
    {
        [DataMember(Name = "old_path")]
        [JsonPropertyName("old_path")]
        public string OldPath { get; set; }

        [DataMember(Name = "new_path")]
        [JsonPropertyName("new_path")]
        public string NewPath { get; set; }

        [DataMember(Name = "a_mode")]
        [JsonPropertyName("a_mode")]
        public long AMode { get; set; }

        [DataMember(Name = "b_mode")]
        [JsonPropertyName("b_mode")]
        public long BMode { get; set; }

        [DataMember(Name = "new_file")]
        [JsonPropertyName("new_file")]
        public bool NewFile { get; set; }

        [DataMember(Name = "renamed_file")]
        [JsonPropertyName("renamed_file")]
        public bool RenamedFile { get; set; }

        [DataMember(Name = "deleted_file")]
        [JsonPropertyName("deleted_file")]
        public bool DeletedFile { get; set; }

        [DataMember(Name = "diff")]
        [JsonPropertyName("diff")]
        public string Diff { get; set; }
    }
}
