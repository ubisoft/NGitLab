using System;

namespace NGitLab.Models;

public readonly struct ProjectId : IIdOrPathAddressable
{
    private readonly long _id;
    private readonly string _path;

    long IIdOrPathAddressable.Id => _id;

    string IIdOrPathAddressable.Path => _path;

    public ProjectId(long id)
    {
        _id = id;
    }

    public ProjectId(string path)
    {
        _path = path ?? throw new ArgumentNullException(nameof(path));
    }

    public ProjectId(Project project)
    {
        _id = project?.Id ?? throw new ArgumentNullException(nameof(project));
    }

    public static implicit operator ProjectId(long id) => new(id);

    public static implicit operator ProjectId(string path) => new(path);

    public static implicit operator ProjectId(Project project) => new(project);

    public override string ToString() => this.ValueAsString();
}
