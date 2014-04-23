using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    public class Commit
    {
        public const string Url = "/commits";

        public string Id;
        public string Title;

        [DataMember(Name = "short_id")]
        public string ShortId;

        [DataMember(Name = "author_name")]
        public string AuthorName;

        [DataMember(Name = "author_email")]
        public string AuthorEmail;

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt;
    }
}