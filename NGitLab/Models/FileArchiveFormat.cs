using System.Runtime.Serialization;

namespace NGitLab.Models;

public enum FileArchiveFormat
{
    [EnumMember(Value = ".bz2")]
    Bz2,

    [EnumMember(Value = ".gz")]
    Gz,

    [EnumMember(Value = ".tar")]
    Tar,

    [EnumMember(Value = ".tar.bz2")]
    TarBz2,

    [EnumMember(Value = ".tar.gz")]
    TarGz,

    [EnumMember(Value = ".tb2")]
    Tb2,

    [EnumMember(Value = ".tbz")]
    Tbz,

    [EnumMember(Value = ".tbz2")]
    Tbz2,

    [EnumMember(Value = ".zip")]
    Zip,
}
