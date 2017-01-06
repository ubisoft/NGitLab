using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class BranchRemovalStatus
    {
        [DataMember(Name = "branch_name")]
        public string Name;
    }
}