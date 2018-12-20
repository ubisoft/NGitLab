using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MergeRequestDiscussion
    {
        [DataMember(Name = "individual_note")]
        public bool IsIndividualNote;

        [DataMember(Name = "notes")]
        public MergeRequestNote[] Notes;
    }
}