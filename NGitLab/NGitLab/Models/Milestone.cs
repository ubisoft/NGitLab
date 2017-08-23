using System;
using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class Milestone {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "due_date")]
        public string DueDate { get; set; }

        [DataMember(Name = "state")]
        public string State { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}