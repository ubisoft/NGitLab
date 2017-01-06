using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestComment
    {
        [DataMember(Name = "note")]
        public string Note;

        [DataMember(Name = "author")]
        public User Author { get; set; }
    }
}