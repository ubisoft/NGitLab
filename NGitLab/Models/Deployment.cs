using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
#pragma warning disable CA1724  // Type names should not match .NET Framework class library namespaces
    public class Deployment
#pragma warning restore CA1724  // Type names should not match .NET Framework class library namespaces
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("iid")]
        public int DeploymentId { get; set; }

        [JsonPropertyName("ref")]
        public string Ref { get; set; }

        [JsonPropertyName("environment")]
        public EnvironmentInfo Environment { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
