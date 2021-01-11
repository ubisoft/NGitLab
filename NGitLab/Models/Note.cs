using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Note
    {
        [DataMember(Name = "id")]
        public long Id;

        [DataMember(Name = "body")]
        public string Body;

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt;

        [DataMember(Name = "author")]
        public User Author { get; set; }

        [DataMember(Name = "resolved")]
        public bool Resolved;

        [DataMember(Name = "resolvable")]
        public bool Resolvable;

        [DataMember(Name = "type")]
        public string Type;

        [DataMember(Name = "system")]
        public bool System;
    }
}
