using System.Runtime.Serialization;

namespace NGitLab.Models;

public enum PositionType
{
    [EnumMember(Value = "text")]
    Text,
    [EnumMember(Value = "image")]
    Image,
}
