using System;

namespace NGitLab.Models
{
    public readonly struct GroupId : IidOrPathAddressable
    {
        private readonly long _id;
        private readonly string _path;

        long IidOrPathAddressable.Id => _id;

        string IidOrPathAddressable.Path => _path;

        public GroupId(long id)
        {
            _id = id;
        }

        public GroupId(string path)
        {
            _path = path ?? throw new ArgumentNullException(nameof(path));
        }

        public GroupId(Group group)
        {
            _id = group?.Id ?? throw new ArgumentNullException(nameof(group));
        }

        public static implicit operator GroupId(long id) => new(id);

        public static implicit operator GroupId(string path) => new(path);

        public static implicit operator GroupId(Group group) => new(group);

        public override string ToString() => this.ValueAsString();
    }
}
