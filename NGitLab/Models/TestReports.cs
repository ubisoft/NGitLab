using System.Collections.Generic;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class TestReports
    {
        [DataMember(Name = "total_time")]
        public int Total_time { get; set; }
        [DataMember(Name = "total_count")]
        public int Total_count { get; set; }
        [DataMember(Name = "success_count")]
        public int Success_count { get; set; }
        [DataMember(Name = "failed_count")]
        public int Failed_count { get; set; }
        [DataMember(Name = "skipped_count")]
        public int Skipped_count { get; set; }
        [DataMember(Name = "error_count")]
        public int Error_count { get; set; }
        [DataMember(Name = "test_suites")]
        public List<TestCases> Test_suites { get; set; }
    }
}
