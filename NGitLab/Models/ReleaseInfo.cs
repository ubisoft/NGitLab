using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ReleaseInfo
    {
        [DataMember(Name = "tag_name")]
        public string TagName;

        [DataMember(Name = "description")]
        public string Description;
    }
}
