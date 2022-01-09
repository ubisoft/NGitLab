using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    [DebuggerDisplay("{Path} ({Type})")]
    public class Tree
    {
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public Sha1 Id;

        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name;

        [DataMember(Name = "type")]
        [JsonPropertyName("type")]
        public ObjectType Type;

        [DataMember(Name = "mode")]
        [JsonPropertyName("mode")]
        public string Mode;

        [DataMember(Name = "path")]
        [JsonPropertyName("path")]
        public string Path;
    }
}
