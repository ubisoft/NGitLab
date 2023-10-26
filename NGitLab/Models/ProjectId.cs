namespace NGitLab.Models
{
    public record ProjectId : IdOrNamespacedPath
    {
        public ProjectId(long id)
            : base(id)
        {
        }

        public ProjectId(string path)
            : base(path)
        {
        }

        public ProjectId(Project project)
            : base(project.Id)
        {
        }

        public static implicit operator ProjectId(long id) => new(id);

        public static implicit operator ProjectId(string path) => new(path);

        public static implicit operator ProjectId(Project project) => new(project);
    }
}
