using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class Permissions
    {
        [JsonPropertyName("project_access")]
        public Permission ProjectAccess { get; set; }

        [JsonPropertyName("group_access")]
        public Permission GroupAccess { get; set; }
    }
}
