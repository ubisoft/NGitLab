using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class PersonInfo
    {
        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "email")]
        public string Email;
    }
}