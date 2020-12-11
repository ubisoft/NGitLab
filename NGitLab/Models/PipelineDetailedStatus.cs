using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class PipelineDetailedStatus
    {
        [DataMember(Name = "icon")]
        public string Icon { get; set; }

        [DataMember(Name = "text")]
        public string Text { get; set; }

        [DataMember(Name = "label")]
        public string Label { get; set; }

        [DataMember(Name = "group")]
        public string Group { get; set; }

        [DataMember(Name = "tooltip")]
        public string ToolTip { get; set; }

        [DataMember(Name = "has_details")]
        public bool HasDetails { get; set; }

        [DataMember(Name = "details_path")]
        public string DetailsPath { get; set; }

        [DataMember(Name = "illustration")]
        public string Illustration { get; set; }

        [DataMember(Name = "favicon")]
        public string FavIcon { get; set; }
    }
}
