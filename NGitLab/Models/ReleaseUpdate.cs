using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class ReleaseUpdate
{
    /// <summary>
    /// (required) - The Git tag the release is associated with.
    /// </summary>
    [Required]
    [JsonPropertyName("tag_name")]
    public string TagName { get; set; }

    /// <summary>
    /// (optional) - The description of the release.
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; }

    /// <summary>
    /// (optional) - The release name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    ///  - The title of each milestone the release is associated with.
    /// </summary>
    [JsonPropertyName("milestones")]
    public string[] Milestones { get; set; }

    /// <summary>
    ///  - The date when the release is/was ready. Defaults to the current time.
    /// </summary>
    [JsonPropertyName("released_at")]
    public DateTime? ReleasedAt { get; set; }
}
