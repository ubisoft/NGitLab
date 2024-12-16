using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class ProjectPermissions
{
    [JsonPropertyName("project_access")]
    public ProjectPermission ProjectAccess { get; set; }

    [JsonPropertyName("group_access")]
    public ProjectPermission GroupAccess { get; set; }
}
