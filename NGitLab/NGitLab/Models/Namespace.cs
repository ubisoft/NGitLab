using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    public class Namespace
    {
        public const string URL = "/groups";

        public int Id;
        public string Name;
        public string Path;
        public string Description;

        [DataMember(Name="created_at")]
        public DateTime CreatedAt;

        [DataMember(Name="updated_at")]
        public DateTime UpdatedAt;

        [DataMember(Name="owner_id")]
        public int OwnerId;
    }
}