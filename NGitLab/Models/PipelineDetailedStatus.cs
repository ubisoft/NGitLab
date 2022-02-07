using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class PipelineDetailedStatus
    {
        [DataMember(Name = "icon")]
        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [DataMember(Name = "text")]
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [DataMember(Name = "label")]
        [JsonPropertyName("label")]
        public string Label { get; set; }

        [DataMember(Name = "group")]
        [JsonPropertyName("group")]
        public string Group { get; set; }

        [DataMember(Name = "tooltip")]
        [JsonPropertyName("tooltip")]
        public string ToolTip { get; set; }

        [DataMember(Name = "has_details")]
        [JsonPropertyName("has_details")]
        public bool HasDetails { get; set; }

        [DataMember(Name = "details_path")]
        [JsonPropertyName("details_path")]
        public string DetailsPath { get; set; }

        [DataMember(Name = "illustration")]
        [JsonPropertyName("illustration")]
        public string Illustration { get; set; }

        [DataMember(Name = "favicon")]
        [JsonPropertyName("favicon")]
        public string FavIcon { get; set; }
    }
}
