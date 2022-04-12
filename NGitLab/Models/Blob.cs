using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class Blob
    {
        [JsonPropertyName("size")]
        public int Size;

        [JsonPropertyName("encoding")]
        public string Encoding;

        [JsonPropertyName("content")]
        public string Content;

        [JsonPropertyName("sha")]
        public Sha1 Sha;
    }
}
