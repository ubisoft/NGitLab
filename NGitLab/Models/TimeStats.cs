using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class TimeStats
    {
        [JsonPropertyName("human_time_estimate")]
        public string HumanTimeEstimate { get; set; }

        [JsonPropertyName("human_total_time_spent")]
        public string HumanTotalTimeSpent { get; set; }

        [JsonPropertyName("time_estimate")]
        public long TimeEstimateInSec { get; set; }

        [JsonPropertyName("total_time_spent")]
        public long TotalTimeSpentInSec { get; set; }
    }
}
