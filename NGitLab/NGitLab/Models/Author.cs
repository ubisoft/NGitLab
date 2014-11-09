using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Author
    {
        [DataMember(Name = "id")]
        public int Id;

        [DataMember(Name = "username")]
        public string Username;

        [DataMember(Name = "email")]
        public string Email;

        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "state")]
        public string State;

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt;
    }
}
