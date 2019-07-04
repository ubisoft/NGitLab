using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestCommentEdit
    {
        [DataMember(Name = "body")]
        public string Body;
    }
}
