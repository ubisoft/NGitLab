using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class RealeaseInfo
    {
        [DataMember(Name = "description")]
        public string Description;
    }
}