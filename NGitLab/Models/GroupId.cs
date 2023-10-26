namespace NGitLab.Models
{
    public record GroupId : IdOrNamespacedPath
    {
        public GroupId(long id)
            : base(id)
        {
        }

        public GroupId(string path)
            : base(path)
        {
        }

        public GroupId(Group group)
            : base(group.Id)
        {
        }

        public static implicit operator GroupId(long id) => new(id);

        public static implicit operator GroupId(string path) => new(path);

        public static implicit operator GroupId(Group group) => new(group);
    }
}
