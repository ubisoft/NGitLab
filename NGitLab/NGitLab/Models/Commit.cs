using System;
using Newtonsoft.Json;

namespace NGitLab.Models
{
    public class Commit
    {
        public const string Url = "/commits";

        public string Id;
        public string Title;

        [JsonProperty("short_id")]
        public string ShortId;

        [JsonProperty("author_name")]
        public string AuthorName;

        [JsonProperty("author_email")]
        public string AuthorEmail;

        [JsonProperty("created_at")]
        public DateTime CreatedAt;
    }
}