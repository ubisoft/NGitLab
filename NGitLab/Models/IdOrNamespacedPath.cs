using System;
using NGitLab.Extensions;

namespace NGitLab.Models
{
    public record IdOrNamespacedPath
    {
        private readonly long _id;
        private readonly string _path;

        public IdOrNamespacedPath(long id) => _id = id;

        public IdOrNamespacedPath(string path) => _path = path ?? throw new ArgumentNullException(nameof(path));

        public static implicit operator IdOrNamespacedPath(long id) => new(id);

        public static implicit operator IdOrNamespacedPath(string path) => new(path);

        internal string ValueAsString => _path ?? _id.ToStringInvariant();

        internal string ValueAsUriParameter => _path is null ? _id.ToStringInvariant() : Uri.EscapeDataString(_path);

        public override string ToString() => ValueAsString;
    }
}
