using System;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class Milestone
{
    [JsonPropertyName("id")]
    public long Id;

    [JsonPropertyName("iid")]
    public long Iid;

    [JsonPropertyName("title")]
    public string Title;

    [JsonPropertyName("description")]
    public string Description;

    [JsonPropertyName("due_date")]
    public string DueDate;

    [JsonPropertyName("group_id")]
    public long? GroupId;

    [JsonPropertyName("project_id")]
    public long? ProjectId;

    [JsonPropertyName("start_date")]
    public string StartDate;

    [JsonPropertyName("state")]
    public string State;

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt;

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt;
}

public enum MilestoneState
{
    active,
    closed,
}
