using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class CommitCreate
    {
        [Obsolete("'Project Id' is ignored in the POSTed JSON; it is actually specified through the endpoint URL.")]
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
