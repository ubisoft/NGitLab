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
        [JsonPropertyName("id")]
        public int ProjectId;

        [Required]
        [JsonPropertyName("branch")]
        public string Branch;

        [JsonPropertyName("commit_message")]
        public string CommitMessage;

        [JsonPropertyName("start_branch")]
        public string StartBranch;

        [JsonPropertyName("author_email")]
        public string AuthorEmail;

        [JsonPropertyName("author_name")]
        public string AuthorName;

        [JsonPropertyName("actions")]
        public IList<CreateCommitAction> Actions = new List<CreateCommitAction>();

        [JsonPropertyName("force")]
        public bool? Force;
    }

    [DataContract]
    public class CreateCommitAction
    {
        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("file_path")]
        public string FilePath { get; set; }

        [JsonPropertyName("previous_path")]
        public string PreviousPath { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; }

        [JsonPropertyName("encoding")]
        public string Encoding { get; set; }
    }
}
