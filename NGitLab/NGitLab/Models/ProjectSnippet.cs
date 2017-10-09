using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ProjectSnippet
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "title")]
        public string Title { get; set; }
        [DataMember(Name = "file_name")]
        public string FileName { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt { get; set; }
        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }
        [DataMember(Name = "web_url")]
        public string WebUrl { get; set; }
        [DataMember(Name = "author")]
        public Author Author { get; set; }

       
    }
}