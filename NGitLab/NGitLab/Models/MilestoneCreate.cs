using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MilestoneCreate
    {
        [DataMember(Name = "title")]
        public string Title;

        [DataMember(Name = "description")]
        public string Description;

        [DataMember(Name = "due_date")]
        public string DueDate;

        [DataMember(Name = "start_date")]
        public string StartDate;
    }
}
