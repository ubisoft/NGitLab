using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class FileUpsert
    {
        [Required]
        [JsonIgnore]
        public string Path;

        [Required]
        [DataMember(Name = "branch")]
        [JsonPropertyName("branch")]
        public string Branch;

        [DataMember(Name = "encoding")]
        [JsonPropertyName("encoding")]
        public string Encoding;

        [Required]
        [DataMember(Name = "content")]
        [JsonPropertyName("content")]
        public string Content;

        /// <summary>
        /// Use this setter to set the content as base 64.
        /// </summary>
        public string RawContent
        {
            set
            {
                Content = Base64Encode(value);
                Encoding = "base64";
            }
        }

        [Required]
        [DataMember(Name = "commit_message")]
        [JsonPropertyName("commit_message")]
        public string CommitMessage;

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}
