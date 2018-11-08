using System.Runtime.Serialization;

namespace NGitLab.Models
{

    [DataContract]
    public class VariableUpdate
    {
        [DataMember(Name = "value")]
        public string Value;

        [DataMember(Name = "protected")]
        public bool Protected;
    }
}
