using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class LastActivityDate
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("last_activity_on")]
        public DateTimeOffset LastActivityOn { get; set; }
    }
}
