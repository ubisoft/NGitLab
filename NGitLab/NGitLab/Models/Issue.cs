using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Issue
    {
        [DataMember(Name = "id")]
        public int Id;

        [DataMember(Name = "iid")]
        public int IssueId;

        [DataMember(Name = "project_id")]
        public int ProjectId;

        [DataMember(Name = "title")]
        public string Title;

        [DataMember(Name = "description")]
        public string Description;

        [DataMember(Name = "labels")]
        public string[] Labels;

        [DataMember(Name = "milestone")]
        public Milestone Milestone;

        [DataMember(Name = "assignee")]
        public Assignee Assignee;

        [DataMember(Name = "author")]
        public Author Author;

        [DataMember(Name = "state")]
        public string State;

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt;
    }
}
