using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestUserInfo
    {
        [DataMember(Name = "can_merge")]
        public bool CanMerge { get; set; }
    }
}
