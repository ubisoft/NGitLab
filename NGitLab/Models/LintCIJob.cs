using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class LintCIJob
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("stage")]
    public string Stage { get; set; }

    [JsonPropertyName("before_script")]
    public string[] BeforeScript { get; set; }

    [JsonPropertyName("script")]
    public string[] Script { get; set; }

    [JsonPropertyName("after_script")]
    public string[] AfterScript { get; set; }

    [JsonPropertyName("tag_list")]
    public string[] TagList { get; set; }

    [JsonPropertyName("environment")]
    public string Environment { get; set; }

    [JsonPropertyName("when")]
    public string When { get; set; }

    [JsonPropertyName("allow_failure")]
    public bool AllowFailure { get; set; }
}
