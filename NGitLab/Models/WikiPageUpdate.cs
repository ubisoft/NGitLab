using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class WikiPageUpdate
    {
        [DataMember(Name = "format")]
        [JsonPropertyName("format")]
        public string Format;

        [DataMember(Name = "content")]
        [JsonPropertyName("content")]
        public string Content;

        [DataMember(Name = "title")]
        [JsonPropertyName("title")]
        public string Title;
    }
}
