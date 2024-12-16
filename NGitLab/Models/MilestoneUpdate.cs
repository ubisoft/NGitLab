using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class MilestoneUpdate
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("due_date")]
    public string DueDate { get; set; }

    [JsonPropertyName("start_date")]
    public string StartDate { get; set; }
}

public class MilestoneUpdateState
{
    [JsonPropertyName("state_event")]
    public string NewState { get; set; }
}

// ReSharper disable InconsistentNaming
public enum MilestoneStateEvent
{
    activate,
    close,
}
