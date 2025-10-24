using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public sealed record TestReportSummary
{
    [JsonPropertyName("total")]
    public Totals Total { get; set; }

    [JsonPropertyName("test_suites")]
    public IReadOnlyCollection<TestSuite> TestSuites { get; set; }

    public sealed record Totals
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

    public sealed record TestSuite
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("total_time")]
        public double TotalTime { get; set; }

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
    }
}
