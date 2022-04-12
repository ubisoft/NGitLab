using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class MilestoneCreate
    {
        [JsonPropertyName("title")]
        public string Title;

        [JsonPropertyName("description")]
        public string Description;

        [JsonPropertyName("due_date")]
        public string DueDate;

        [JsonPropertyName("start_date")]
        public string StartDate;
    }
}
