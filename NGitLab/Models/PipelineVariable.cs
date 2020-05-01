using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class PipelineVariable
    {
        [DataMember(Name = "key")]
        public string Key;

        [DataMember(Name = "value")]
        public string Value;

        [DataMember(Name = "variable_type")]
        public string VariableType;
    }
}
