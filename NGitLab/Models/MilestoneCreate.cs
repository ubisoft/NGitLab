using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MilestoneCreate
    {
        [DataMember(Name = "title")]
        [JsonPropertyName("title")]
        public string Title;

        [DataMember(Name = "description")]
        [JsonPropertyName("description")]
        public string Description;

        [DataMember(Name = "due_date")]
        [JsonPropertyName("due_date")]
        public string DueDate;

        [DataMember(Name = "start_date")]
        [JsonPropertyName("start_date")]
        public string StartDate;
    }
}
