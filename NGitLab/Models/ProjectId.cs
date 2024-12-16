using System;

namespace NGitLab.Models;

/// <summary>
/// A lightweight struct containing either a <see cref="Project"/> Id or Path, but not both.
/// </summary>
/// <remarks>
/// Because a <see cref="ProjectId"/> contains an Id -or- a Path, two <see cref="ProjectId"/>s
/// are only "equal" when they both contain the same Id -or- the same Path; they will NOT be
/// "equal" when one contains an Id and the other contains a Path.
/// For this reason it is preferable to compare <see cref="ProjectId"/> to <see cref="Project"/>.
/// </para>
/// Path comparison is case-insensitive.
/// </remarks>
public readonly struct ProjectId : IIdOrPathAddressable, IEquatable<Project>, IEquatable<ProjectId>
{
    private readonly long _id;
    private readonly string _path;

    long IIdOrPathAddressable.Id => _id;

    string IIdOrPathAddressable.Path => _path;

    public ProjectId(long id)
    {
        _id = id != 0 ? id : throw new ArgumentException("Id cannot be zero", nameof(id));
    }

    public ProjectId(string path)
    {
        _path = path ?? throw new ArgumentNullException(nameof(path));
    }

    public static implicit operator ProjectId(long id) => new(id);

    public static implicit operator ProjectId(string path) => new(path);

    public override string ToString() => this.ValueAsString();

    public override int GetHashCode() =>
        _path != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(_path) : _id.GetHashCode();

    /// <summary>
    /// Checks if this <see cref="ProjectId"/> matches the given object.
    /// <summary>
    /// <param name="other">The other object.</param>
    /// <returns>True if <paramref name="other"/> is a matching <see cref="ProjectId"/> or <see cref="Project"/>, otherwise false.</returns>
    /// <seealso cref="Equals(ProjectId)"/> <seealso cref="Equals(Project)"/>
    public override bool Equals(object other) =>
        ((other is ProjectId id) && Equals(id)) ||
        ((other is Project project) && Equals(project));

    /// <summary>
    /// Checks if two <see cref="ProjectId"/>s are equivalent by comparing Path-to-Path or Id-to-Id,
    /// depending on the kind of identifier value stored in each ProjectId.
    /// </para>
    /// It is important to understand that a ProjectId contains an Id -or- a Path, not both.
    /// Thus two ProjectIds which identify the same project will NOT be "equal"
    /// when one contains the project's Id and the other contains the project's Path.
    /// <summary>
    /// <remarks>
    /// Path comparison is case-insensitive.
    /// </remarks>
    /// <param name="other">The other identifier.</param>
    /// <returns>True if the ProjectIds are equivalent (Paths match or Ids match), otherwise false.</returns>
    public bool Equals(ProjectId other) =>
        _path != null
        ? other._path != null && StringComparer.OrdinalIgnoreCase.Equals(_path, other._path)
        : other._path == null && _id == other._id;

    /// <summary>
    /// Checks if this <see cref="ProjectId"/> matches the given <see cref="Project"/>'s Path or Id.
    /// <summary>
    /// <remarks>
    /// Path comparison is case-insensitive.
    /// </remarks>
    /// <param name="other">The other identifier.</param>
    /// <returns>True if the ProjectId match the Project, otherwise false.</returns>
    public bool Equals(Project other) =>
        other != null && Equals(other.PathWithNamespace, other.Id);

    /// <summary>
    /// Checks if this <see cref="ProjectId"/> matches the given Path or Id.
    /// <summary>
    /// <remarks>
    /// Path comparison is case-insensitive.
    /// </remarks>
    /// <param name="otherPath">The other full path.</param>
    /// <param name="otherId">The other id.</param>
    /// <returns>True if the ProjectId match the Project, otherwise false.</returns>
    public bool Equals(string otherPath, long otherId) =>
        _path != null ? StringComparer.OrdinalIgnoreCase.Equals(_path, otherPath) : _id == otherId;

    /// <inheritdoc cref="Equals(ProjectId)"/>
    public static bool operator ==(ProjectId a, ProjectId b) => a.Equals(b);

    /// <inheritdoc cref="Equals(ProjectId)"/>
    public static bool operator !=(ProjectId a, ProjectId b) => !a.Equals(b);

    /// <inheritdoc cref="Equals(Project)"/>
    public static bool operator ==(ProjectId a, Project b) => a.Equals(b);

    /// <inheritdoc cref="Equals(Project)"/>
    public static bool operator !=(ProjectId a, Project b) => !a.Equals(b);

    /// <inheritdoc cref="Equals(Project)"/>
    public static bool operator ==(Project a, ProjectId b) => b.Equals(a);

    /// <inheritdoc cref="Equals(Project)"/>
    public static bool operator !=(Project a, ProjectId b) => !b.Equals(a);
}
