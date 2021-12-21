using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class SshKeyCreate
    {
        [DataMember(Name = "title")]
        public string Title;

        [DataMember(Name = "key")]
        public string Key;
    }
}
