using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Variable
    {
        [DataMember(Name = "key")]
        public string Key;

        [DataMember(Name = "value")]
        public string Value;

        [DataMember(Name = "protected")]
        public bool Protected;
    }
}
