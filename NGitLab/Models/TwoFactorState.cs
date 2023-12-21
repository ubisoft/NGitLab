using System.Runtime.Serialization;

namespace NGitLab.Models;

public enum TwoFactorState
{
    [EnumMember(Value = "enabled")]
    Enabled,
    [EnumMember(Value = "disabled")]
    Disabled,
}
