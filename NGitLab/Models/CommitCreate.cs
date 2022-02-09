using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class CommitCreate
    {
        [Required]
        [DataMember(Name = "id")]
        [JsonPropertyName("id")]
        public int ProjectId;

        [Required]
        [DataMember(Name = "branch")]
        [JsonPropertyName("branch")]
        public string Branch;

        [DataMember(Name = "commit_message")]
        [JsonPropertyName("commit_message")]
        public string CommitMessage;

        [DataMember(Name = "start_branch")]
        [JsonPropertyName("start_branch")]
        public string StartBranch;

        [DataMember(Name = "author_email")]
        [JsonPropertyName("author_email")]
        public string AuthorEmail;

        [DataMember(Name = "author_name")]
        [JsonPropertyName("author_name")]
        public string AuthorName;

        [DataMember(Name = "actions")]
        [JsonPropertyName("actions")]
        public IList<CreateCommitAction> Actions = new List<CreateCommitAction>();

        [DataMember(Name = "force")]
        [JsonPropertyName("force")]
        public bool? Force;
    }

    [DataContract]
    public class CreateCommitAction
    {
        [DataMember(Name = "action")]
        [JsonPropertyName("action")]
        public string Action { get; set; }

        [DataMember(Name = "file_path")]
        [JsonPropertyName("file_path")]
        public string FilePath { get; set; }

        [DataMember(Name = "previous_path")]
        [JsonPropertyName("previous_path")]
        public string PreviousPath { get; set; }

        [DataMember(Name = "content")]
        [JsonPropertyName("content")]
        public string Content { get; set; }

        [DataMember(Name = "encoding")]
        [JsonPropertyName("encoding")]
        public string Encoding { get; set; }
    }
}
