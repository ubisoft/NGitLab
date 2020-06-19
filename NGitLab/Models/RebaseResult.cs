using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class RebaseResult
    {
        [DataMember(Name = "rebase_in_progress")]
        public bool RebaseInProgress;
    }
}
