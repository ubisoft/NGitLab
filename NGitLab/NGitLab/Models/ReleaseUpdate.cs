using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ReleaseUpdate
    {
        [DataMember(Name = "description")]
        public string Description;
    }
}
