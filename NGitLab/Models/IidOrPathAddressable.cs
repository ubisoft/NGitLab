#nullable enable

namespace NGitLab.Models
{
    public interface IidOrPathAddressable
    {
        internal long Id { get; }

        internal string? Path { get; }
    }
}
