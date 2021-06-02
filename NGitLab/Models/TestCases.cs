using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class TestCases
    {
        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "classname")]
        public string Classname { get; set; }

        [DataMember(Name = "execution_time")]
        public int ExecutionTime { get; set; }

        [DataMember(Name = "system_ouput")]
        public string SystemOutput { get; set; }

        [DataMember(Name = "stack_trace")]
        public string StrackTrace { get; set; }
    }
}
