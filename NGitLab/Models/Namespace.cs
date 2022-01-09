using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Namespace
    {
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id;

        [DataMember(Name = "name")]
        [JsonPropertyName("name")]
        public string Name;

        [DataMember(Name = "path")]
        [JsonPropertyName("path")]
        public string Path;

        [DataMember(Name = "kind")]
        [JsonPropertyName("kind")]
        public string Kind;

        [DataMember(Name = "full_path")]
        [JsonPropertyName("full_path")]
        public string FullPath;

        public enum Type
        {
            Group,
            User,
        }

        public Type GetKind()
        {
            return (Type)Enum.Parse(typeof(Type), Kind, ignoreCase: true);
        }
    }
}
