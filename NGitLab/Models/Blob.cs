using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Blob
    {
        [DataMember(Name = "size")]
        [JsonPropertyName("size")]
        public int Size;

        [DataMember(Name = "encoding")]
        [JsonPropertyName("encoding")]
        public string Encoding;

        [DataMember(Name = "content")]
        [JsonPropertyName("content")]
        public string Content;

        [DataMember(Name = "sha")]
        [JsonPropertyName("sha")]
        public Sha1 Sha;
    }
}
