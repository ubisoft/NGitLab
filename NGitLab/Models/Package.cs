using System;
using System.Text.Json.Serialization;
using NGitLab.Impl.Json;

namespace NGitLab.Models
{
    public class Package
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("package_id")]
        public int PackageId { get; set; }

        [JsonPropertyName("created_at")]
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime? UpdatedAt { get; set; }

        [JsonPropertyName("size")]
        public int Size { get; set; }

        [JsonPropertyName("file_store")]
        public int FileStore { get; set; }

        [JsonPropertyName("file_md5")]
        public string FileMD5 { get; set; }

        [JsonPropertyName("file_sha1")]
        public string FileSHA1 { get; set; }

        [JsonPropertyName("file_sha256")]
        public string FileSHA256 { get; set; }

        [JsonPropertyName("file_name")]
        public string FileName { get; set; }

        [JsonPropertyName("verification_retry_at")]
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime? VerificationRetryAt { get; set; }

        [JsonPropertyName("verified_at")]
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime? VerifiedAt { get; set; }

        [JsonPropertyName("verification_failure")]
        public string VerificationFailure { get; set; }

        [JsonPropertyName("verification_retry_count")]
        public string VerificationRetryCount { get; set; }

        [JsonPropertyName("verification_checksum")]
        public string VerificationChecksum { get; set; }

        [JsonPropertyName("verification_state")]
        public int VerificationState { get; set; }

        [JsonPropertyName("verification_started_at")]
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime? VerificationStartedAt { get; set; }

        [JsonPropertyName("new_file_path")]
        public string NewFilePath { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("file")]
        public PackageFile File { get; set; }
    }
}
