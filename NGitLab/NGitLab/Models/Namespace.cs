using System;
using Newtonsoft.Json;

namespace NGitLab.Models
{
    public class Namespace
    {
        public const string URL = "/groups";

        public int Id;
        public string Name;
        public string Path;
        public string Description;

        [JsonProperty("created_at")]
        public DateTime CreatedAt;

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt;

        [JsonProperty("owner_id")]
        public int OwnerId;
    }
}