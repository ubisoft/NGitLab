using System.Text.Json.Serialization;

namespace NGitLab.Models;

public sealed class TestReportSummaryTotals
{
    [JsonPropertyName("time")]
    public double Time { get; set; }

    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("success")]
    public int Success { get; set; }

    [JsonPropertyName("failed")]
    public int Failed { get; set; }

    [JsonPropertyName("skipped")]
    public int Skipped { get; set; }

    [JsonPropertyName("error")]
    public int Error { get; set; }
}
