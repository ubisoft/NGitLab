using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Diff
    {
        [DataMember(Name = "diff")]
        [JsonPropertyName("diff")]
        public string Difference;

        [DataMember(Name = "new_path")]
        [JsonPropertyName("new_path")]
        public string NewPath;

        [DataMember(Name = "old_path")]
        [JsonPropertyName("old_path")]
        public string OldPath;

        [DataMember(Name = "a_mode")]
        [JsonPropertyName("a_mode")]
        public string AMode;

        [DataMember(Name = "b_mode")]
        [JsonPropertyName("b_mode")]
        public string BMode;

        [DataMember(Name = "new_file")]
        [JsonPropertyName("new_file")]
        public bool IsNewFile;

        [DataMember(Name = "renamed_file")]
        [JsonPropertyName("renamed_file")]
        public bool IsRenamedFile;

        [DataMember(Name = "deleted_file")]
        [JsonPropertyName("deleted_file")]
        public bool IsDeletedFile;
    }
}
