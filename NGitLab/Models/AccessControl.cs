using System.Text.Json.Serialization;

namespace NGitLab.Models;

public abstract class AccessControl
{
}

public class UserIdControl : AccessControl
{
    [JsonPropertyName("user_id")]
    public long UserId { get; set; }
}

public class GroupIdControl : AccessControl
{
    [JsonPropertyName("group_id")]
    public long GroupId { get; set; }
}

public class AccessLevelControl : AccessControl
{
    [JsonPropertyName("access_level")]
    public AccessLevel AccessLevel { get; set; }
}
