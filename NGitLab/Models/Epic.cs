using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Epic
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "iid")]
        public int EpicIid { get; set; }

        [DataMember(Name = "group_id")]
        public int GroupId { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "labels")]
        public string[] Labels { get; set; }

        [DataMember(Name = "state")]
        public EpicState State { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt { get; set; }

        [DataMember(Name = "web_url")]
        public string WebUrl { get; set; }
    }
}
