using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestComment
    {
        [DataMember(Name = "body")]
        public string Body;

        [DataMember(Name = "author")]
        public Author Author { get; set; }
    }
}