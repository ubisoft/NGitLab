using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Identity
    {
        [DataMember(Name = "provider")]
        public string Provider;

        [DataMember(Name = "extern_uid")]
        public string ExternUid;
    }
}
