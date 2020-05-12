using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ResourceLabelEvent
    {

        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "user")]
        public Author User { get; set; }

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "resource_id")]
        public int ResourceId { get; set; }

        [DataMember(Name = "label")]
        public Label Label { get; set; }

        [DataMember(Name = "action")]
        public ResourceLabelEventAction Action { get; set; }
    }
}
