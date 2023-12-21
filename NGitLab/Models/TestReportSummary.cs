using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

public record TestReportSummary
{
    [JsonPropertyName("total")]
    public TestReportSummaryTotals Total { get; set; }

    [JsonPropertyName("test_suites")]
    public IReadOnlyCollection<TestSuites> TestSuites { get; set; }
}
