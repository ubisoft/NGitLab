using System.Runtime.Serialization;

namespace NGitLab.Models
{
    public enum SquashOption
    {
        [EnumMember(Value = "never")]
        Never,

        [EnumMember(Value = "always")]
        Always,

        [EnumMember(Value = "default_off")]
        DefaultOff,

        [EnumMember(Value = "default_on")]
        DefaultOn,
    }
}
