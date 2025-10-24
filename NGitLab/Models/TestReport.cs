using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public sealed record TestReport
{
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

    [JsonPropertyName("test_suites")]
    public IReadOnlyCollection<TestSuite> TestSuites { get; set; }

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

        [JsonPropertyName("test_cases")]
        public IReadOnlyCollection<TestCase> TestCases { get; set; }
    }

    public sealed record TestCase
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("classname")]
        public string Classname { get; set; }

        [JsonPropertyName("file")]
        public string File { get; set; }

        [JsonPropertyName("execution_time")]
        public double ExecutionTime { get; set; }

        [JsonPropertyName("system_output")]
        public string SystemOutput { get; set; }

        [JsonPropertyName("stack_trace")]
        public string StrackTrace { get; set; }
    }
}
