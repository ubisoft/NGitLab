using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace NGitLab.Models {
    [DataContract]
    public class CommitStatus {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public JobStatus? Status { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }
        [DataMember(Name = "started_at")]
        public DateTime StartedAt { get; set; }
        [DataMember(Name = "finished_at")]
        public DateTime FinishedAt { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "allow_failure")]
        public bool AllowFailure { get; set; }
        [DataMember(Name = "author")]
        public User Author { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [DataMember(Name = "sha")]
        public string Sha { get; set; }
        [DataMember(Name = "ref")]
        public string Ref { get; set; }
    }

    public enum JobStatus {
        undefined,
        pending,
        success,
        failed,
        aborted,
        running,
    }

//    {
//    "status" : "pending",
//    "created_at" : "2016-01-19T08:40:25.934Z",
//    "started_at" : null,
//    "name" : "bundler:audit",
//    "allow_failure" : true,
//    "author" : {
//    "username" : "thedude",
//    "state" : "active",
//    "web_url" : "https://gitlab.example.com/thedude",
//    "avatar_url" : "https://gitlab.example.com/uploads/user/avatar/28/The-Big-Lebowski-400-400.png",
//    "id" : 28,
//    "name" : "Jeff Lebowski"
//},
//"description" : null,
//"sha" : "18f3e63d05582537db6d183d9d557be09e1f90c8",
//"target_url" : "https://gitlab.example.com/thedude/gitlab-ce/builds/91",
//"finished_at" : null,
//"id" : 91,
//"ref" : "master"
//},
}