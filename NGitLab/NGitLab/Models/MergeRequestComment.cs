using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestComment
    {
        [DataMember(Name = "body")] 
        public string Body;

        [DataMember(Name = "author")]
        public User Author { get; set; }
    }
}