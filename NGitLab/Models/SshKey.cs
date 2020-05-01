using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class SshKey
    {
        [DataMember(Name = "id")]
        public int Id;

        [DataMember(Name = "title")]
        public string Title;

        [DataMember(Name = "key")]
        public string Key;

        [DataMember(Name = "created_at")]
        public DateTime CreateAt;
    }
}