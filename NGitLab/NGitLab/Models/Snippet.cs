using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Snippet
    {
        [DataMember(Name = "id")]
        public int Id;

        [DataMember(Name = "title")]
        public string Title;

        [DataMember(Name = "file_name")]
        public string FileName;

        [DataMember(Name = "description")]
        public string Description;

        [DataMember(Name = "author")]
        public Author Author;

        [DataMember(Name = "updated_at")]
        public string UpdatedAt;

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "web_url")]
        public string WebUrl;
    }
}
