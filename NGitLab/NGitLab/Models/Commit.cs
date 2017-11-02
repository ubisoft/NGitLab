using System;
using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class Commit {
        public const string Url = "/commits";

        [DataMember(Name = "id")]
        public Sha1 Id { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "short_id")]
        public string ShortId { get; set; }

        [DataMember(Name = "author_name")]
        public string AuthorName { get; set; }

        [DataMember(Name = "author_email")]
        public string AuthorEmail { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "parent_ids")]
        public Sha1[] Parents { get; set; }

    }
}