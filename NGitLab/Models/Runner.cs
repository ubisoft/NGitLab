using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DebuggerDisplay("{Description}")]
    [DataContract]
    public class Runner
    {
        public const string Url = "/runners";

        [DataMember(Name = "id")]
        public int Id;

        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "active")]
        public bool Active;

        [DataMember(Name = "online")]
        public bool Online;

        [DataMember(Name = "description")]
        public string Description;

        [DataMember(Name = "is_shared")]
        public bool IsShared;

        [DataMember(Name = "contacted_at")]
        public DateTime ContactedAt;

        [DataMember(Name = "projects")]
        public Project[] Projects;

        [DataMember(Name = "token")]
        public string Token;

        [DataMember(Name = "tag_list")]
        public string[] TagList;

        [DataMember(Name = "string")]
        public string Version;

        [DataMember(Name = "ip_address")]
        public string IpAddress;

        [DataMember(Name = "locked")]
        public bool Locked;

        [DataMember(Name = "run_untagged")]
        public bool RunUntagged;
    }
}