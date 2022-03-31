using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class WikiPageCreate
    {
        [JsonPropertyName("format")]
        public string Format;

        [JsonPropertyName("content")]
        public string Content;

        [JsonPropertyName("title")]
        public string Title;
    }
}
