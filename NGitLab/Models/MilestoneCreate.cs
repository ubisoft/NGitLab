using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class MilestoneCreate
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
