using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Deployment
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "iid")]
        public int DeploymentId { get; set; }

        [DataMember(Name = "ref")]
        public int Ref { get; set; }

        [DataMember(Name = "environment")]
        public int Environment { get; set; }

        [DataMember(Name = "status")]
        public int Status { get; set; }

        [DataMember(Name = "created_at")]
        public int CreatedAt { get; set; }

        [DataMember(Name = "updated_at")]
        public int UpdatedAt { get; set; }
    }
}
