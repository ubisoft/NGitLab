using System.Runtime.Serialization;

namespace NGitLab.Models;

public enum VariableType
{
    [EnumMember(Value = "env_var")]
    Variable,
    [EnumMember(Value = "file")]
    File,
}
