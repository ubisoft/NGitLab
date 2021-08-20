using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestApprove
    {
        /// <summary>
        /// (optional) - if present, then this SHA must match the HEAD of the source branch, otherwise the merge will fail
        /// </summary>
        [DataMember(Name = "sha")]
        public string Sha { get; set; }

        /// <summary>
        /// (optional) - Current user’s password. Required if Require user password to approve is enabled in the project settings.
        /// </summary>
        [DataMember(Name = "approval_password ")]
        public string ApprovalPassword { get; set; }
    }
}
