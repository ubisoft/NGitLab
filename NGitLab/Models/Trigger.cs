using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Trigger
    {
        [DataMember(Name = "id")]
        public int Id;

        [DataMember(Name = "description")]
        public string Description;

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "last_used")]
        public DateTime LastUsed;

        [DataMember(Name = "token")]
        public string Token;

        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt;
    }
}
