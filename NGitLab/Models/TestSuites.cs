using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class TestSuites
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "total_time")]
        public int TotalTime { get; set; }

        [DataMember(Name = "total_count")]
        public int TotalCount { get; set; }

        [DataMember(Name = "success_count")]
        public int SuccessCount { get; set; }

        [DataMember(Name = "failed_count")]
        public int FailedCount { get; set; }

        [DataMember(Name = "skipped_count")]
        public int SkippedCount { get; set; }

        [DataMember(Name = "error_count")]
        public int ErrorCount { get; set; }

        [DataMember(Name = "test_cases")]
        public IReadOnlyCollection<TestCases> TestCases { get; set; }
    }
}
