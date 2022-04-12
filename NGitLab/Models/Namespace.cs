using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class Namespace
    {
        [JsonPropertyName("id")]
        public int Id;

        [JsonPropertyName("name")]
        public string Name;

        [JsonPropertyName("path")]
        public string Path;

        [JsonPropertyName("kind")]
        public string Kind;

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
