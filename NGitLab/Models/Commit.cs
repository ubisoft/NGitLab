using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Commit
    {
        public const string Url = "/commits";

        [DataMember(Name = "id")]
        public Sha1 Id;

        [DataMember(Name = "title")]
        public string Title;

        [DataMember(Name = "short_id")]
        public string ShortId;

        [DataMember(Name = "author_name")]
        public string AuthorName;

        [DataMember(Name = "author_email")]
        public string AuthorEmail;

        [DataMember(Name = "authored_date")]
        public DateTime AuthoredDate;

        [DataMember(Name = "committer_name")]
        public string CommitterName;

        [DataMember(Name = "committer_email")]
        public string CommitterEmail;

        [DataMember(Name = "committed_date")]
        public DateTime CommittedDate;

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "message")]
        public string Message;

        [DataMember(Name = "parent_ids")]
        public Sha1[] Parents;

        [DataMember(Name = "status")]
        public string Status;

        [DataMember(Name = "stats")]
        public CommitStats Stats;
    }
}
