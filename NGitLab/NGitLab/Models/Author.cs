using System;
using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class Author {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "username")]
        public string Username { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "state")]
        public string State { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }
    }
}