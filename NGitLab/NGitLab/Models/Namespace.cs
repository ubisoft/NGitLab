using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Namespace
    {
        public const string URL = "/groups";

        [DataMember(Name = "id")]
        public int Id;

        [DataMember(Name = "name")]
        public string Name;

        [DataMember(Name = "path")]
        public string Path;

        [DataMember(Name = "description")]
        public string Description;

        [DataMember(Name="created_at")]
        public DateTime CreatedAt;

        [DataMember(Name="updated_at")]
        public DateTime UpdatedAt;

        [DataMember(Name="owner_id")]
        public int? OwnerId;
    }
}
