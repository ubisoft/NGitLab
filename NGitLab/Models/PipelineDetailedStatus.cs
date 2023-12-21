using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class PipelineDetailedStatus
{
    [JsonPropertyName("icon")]
    public string Icon { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }

    [JsonPropertyName("label")]
    public string Label { get; set; }

    [JsonPropertyName("group")]
    public string Group { get; set; }

    [JsonPropertyName("tooltip")]
    public string ToolTip { get; set; }

    [JsonPropertyName("has_details")]
    public bool HasDetails { get; set; }

    [JsonPropertyName("details_path")]
    public string DetailsPath { get; set; }

    [JsonPropertyName("illustration")]
    public string Illustration { get; set; }

    [JsonPropertyName("favicon")]
    public string FavIcon { get; set; }
}
