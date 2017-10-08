using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ProjectSnippetUpdate : ProjectSnippetInsert
    {
        [DataMember(Name = "snippet_id")]
        public int SnippetID { get; set; }
    }
}