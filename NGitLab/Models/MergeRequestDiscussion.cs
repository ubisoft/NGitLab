using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestDiscussion
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "individual_note")]
        public bool IndividualNote { get; set; }

        [DataMember(Name = "notes")]
        public MergeRequestComment[] Notes { get; set; }
    }
}
