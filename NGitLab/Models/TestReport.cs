using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class TestReport
{
    [JsonPropertyName("total_time")]
    public int TotalTime { get; set; }

    [JsonPropertyName("total_count")]
    public int TotalCount { get; set; }

    [JsonPropertyName("success_count")]
    public int SuccessCount { get; set; }

    [JsonPropertyName("failed_count")]
    public int FailedCount { get; set; }

    [JsonPropertyName("skipped_count")]
    public int SkippedCount { get; set; }

    [JsonPropertyName("error_count")]
    public int ErrorCount { get; set; }

    [JsonPropertyName("test_suites")]
    public IReadOnlyCollection<TestSuites> TestSuites { get; set; }
}
