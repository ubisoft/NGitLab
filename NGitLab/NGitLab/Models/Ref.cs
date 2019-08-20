using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Ref
    {
        [DataMember(Name = "type")]
        public string Type;

        [DataMember(Name = "name")]
        public string Name;
    }
}