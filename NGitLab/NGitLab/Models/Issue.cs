using System;
using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class Issue {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "iid")]
        public int IssueId { get; set; }

        [DataMember(Name = "project_id")]
        public int ProjectId { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "labels")]
        public string[] Labels { get; set; }

        [DataMember(Name = "milestone")]
        public Milestone Milestone { get; set; }

        [DataMember(Name = "assignee")]
        public Assignee Assignee { get; set; }

        [DataMember(Name = "author")]
        public Author Author { get; set; }

        [DataMember(Name = "state")]
        public string State { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}