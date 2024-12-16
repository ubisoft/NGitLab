#nullable enable

namespace NGitLab.Models;

public interface IIdOrPathAddressable
{
    internal long Id { get; }

    internal string? Path { get; }
}
