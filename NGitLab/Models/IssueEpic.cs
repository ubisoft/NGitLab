using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class IssueEpic
    {
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [DataMember(Name = "iid")]
        [JsonPropertyName("iid")]
        public int EpicId { get; set; }

        [DataMember(Name = "group_id")]
        [JsonPropertyName("group_id")]
        public int GroupId { get; set; }

        [DataMember(Name = "title")]
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [DataMember(Name = "url")]
        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
