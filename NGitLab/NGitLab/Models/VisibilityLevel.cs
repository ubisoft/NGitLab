using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public enum VisibilityLevel
    {
        [EnumMember(Value = "private")]
        Private = 0,

        [EnumMember(Value = "internal")]
        Internal = 10,

        [EnumMember(Value = "public")]
        Public = 20
    }
}
