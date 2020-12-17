using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
#pragma warning disable CA1724  // Type names should not match .NET Framework class library namespaces
    public class Deployment
#pragma warning restore CA1724  // Type names should not match .NET Framework class library namespaces
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "iid")]
        public int DeploymentId { get; set; }

        [DataMember(Name = "ref")]
        public string Ref { get; set; }

        [DataMember(Name = "environment")]
        public EnvironmentInfo Environment { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
