using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class TimeStats
    {
        [DataMember(Name = "human_time_estimate")]
        public string HumanTimeEstimate { get; set; }

        [DataMember(Name = "human_total_time_spent")]
        public string HumanTotalTimeSpent { get; set; }

        [DataMember(Name = "time_estimate")]
        public long TimeEstimateInSec { get; set; }

        [DataMember(Name = "total_time_spent")]
        public long TotalTimeSpentInSec { get; set; }
    }
}
