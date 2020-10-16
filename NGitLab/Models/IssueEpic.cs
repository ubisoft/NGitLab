using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class IssueEpic
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "iid")]
        public int EpicId { get; set; }

        [DataMember(Name = "group_id")]
        public int GroupId { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; } 

        [DataMember(Name = "url")]
        public string Url { get; set; } 
    }
}
