using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public enum IssuesAccessLevel
    {
        [EnumMember(Value = "disabled")]
        Disabled = 0,

        [EnumMember(Value = "private")]
        Private = 10,

        [EnumMember(Value = "enabled")]
        Enabled = 20
    }
}