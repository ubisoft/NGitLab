using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestApprover
    {
        [DataMember(Name = "user")]
        public User User;
    }
}
