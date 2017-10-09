using System;
using System.Runtime.Serialization;

namespace NGitLab.Models {
    [DataContract]
    public class Namespace {
        public const string Url = "/groups";

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "full_path")]
        public string FullPath { get; set; }
        [DataMember(Name = "path")]
        public string Path { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt { get; set; }

        [DataMember(Name = "owner_id")]
        public int? OwnerId { get; set; }
    }
}