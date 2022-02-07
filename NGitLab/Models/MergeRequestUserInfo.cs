using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestUserInfo
    {
        [DataMember(Name = "can_merge")]
        [JsonPropertyName("can_merge")]
        public bool CanMerge { get; set; }
    }
}
