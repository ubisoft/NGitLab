using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class Permission
    {
        [JsonPropertyName("access_level")]
        public AccessLevel AccessLevel { get; set; }

        [JsonPropertyName("notification_level")]
        public int NotificationLevel { get; set; }
    }
}
