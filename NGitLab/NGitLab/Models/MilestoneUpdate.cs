using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class MilestoneUpdate
    {
        [DataMember(Name = "title")] public string Title;

        [DataMember(Name = "description")] public string Description;

        [DataMember(Name = "due_date")] public string DueDate;

        [DataMember(Name = "start_date")] public string StartDate;
    }

    [DataContract]
    public class MilestoneUpdateState
    {
        [DataMember(Name = "state_event")] public string NewState;
    }

    // ReSharper disable InconsistentNaming
    public enum MilestoneStateEvent
    {
        activate,
        close
    }
}
