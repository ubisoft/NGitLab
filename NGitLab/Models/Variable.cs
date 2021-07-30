using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Variable
    {
        [DataMember(Name = "key")]
        public string Key { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }

        [DataMember(Name = "protected")]
        public bool Protected { get; set; }

        [DataMember(Name = "variable_type")]
        public VariableType Type { get; set; }

        [DataMember(Name = "masked")]
        public bool Masked { get; set; }

        [DataMember(Name = "environment_scope")]
        public string Scope { get; set; }
    }
}
