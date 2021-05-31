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
        public int Execution_time { get; set; }
        [DataMember(Name = "system_ouput")]
        public string System_output { get; set; }
        [DataMember(Name = "stack_trace")]
        public string Strack_trace { get; set; }
    }
}
