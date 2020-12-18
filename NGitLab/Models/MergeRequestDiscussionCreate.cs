using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestDiscussionCreate
    {
        [DataMember(Name = "body")]
        public string Body;

        [DataMember(Name = "created_at")]
        public DateTime? CreatedAt;
    }
}
