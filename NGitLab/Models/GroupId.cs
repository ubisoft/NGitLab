using System;

namespace NGitLab.Models;

/// <summary>
/// A lightweight struct containing either a <see cref="Group"/> Id or a Path, but not both.
/// </summary>
/// <remarks>
/// Because a <see cref="GroupId"/> contains an Id -or- a Path, two <see cref="GroupId"/>s
/// are only "equal" when they both contain the same Id -or- the same Path; they will NOT be
/// "equal" when one contains an Id and the other contains a Path.
/// For this reason it is preferable to compare <see cref="GroupId"/> to <see cref="Group"/>.
/// </para>
/// Path comparison is case-insensitive.
/// </remarks>
public readonly struct GroupId : IIdOrPathAddressable, IEquatable<Group>, IEquatable<GroupId>
{
    private readonly long _id;
    private readonly string _path;

    long IIdOrPathAddressable.Id => _id;

    string IIdOrPathAddressable.Path => _path;

    public GroupId(long id)
    {
        _id = id != 0 ? id : throw new ArgumentException("Id cannot be zero", nameof(id));
    }

    public GroupId(string path)
    {
        _path = path ?? throw new ArgumentNullException(nameof(path));
    }

    public static implicit operator GroupId(long id) => new(id);

    public static implicit operator GroupId(string path) => new(path);

    public override string ToString() => this.ValueAsString();

    public override int GetHashCode() =>
        _path != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(_path) : _id.GetHashCode();

    /// <summary>
    /// Checks if this <see cref="GroupId"/> matches the given object.
    /// <summary>
    /// <param name="other">The other object.</param>
    /// <returns>True if <paramref name="other"/> is a matching <see cref="GroupId"/> or <see cref="Group"/>, otherwise false.</returns>
    /// <seealso cref="Equals(GroupId)"/> <seealso cref="Equals(Group)"/>
    public override bool Equals(object other) =>
        ((other is GroupId id) && Equals(id)) ||
        ((other is Group group) && Equals(group));

    /// <summary>
    /// Checks if two <see cref="GroupId"/>s are equivalent by comparing Path-to-Path or Id-to-Id,
    /// depending on the kind of identifier value stored in each GroupId.
    /// </para>
    /// It is important to understand that a GroupId contains an Id -or- a Path, not both.
    /// Thus two GroupIds which identify the same group will NOT be "equal"
    /// when one contains the group's Id and the other contains the group's Path.
    /// <summary>
    /// <remarks>
    /// Path comparison is case-insensitive.
    /// </remarks>
    /// <param name="other">The other identifier.</param>
    /// <returns>True if the GroupIds are equivalent (Paths match or Ids match), otherwise false.</returns>
    public bool Equals(GroupId other) =>
        _path != null
        ? other._path != null && StringComparer.OrdinalIgnoreCase.Equals(_path, other._path)
        : other._path == null && _id == other._id;

    /// <summary>
    /// Checks if this <see cref="GroupId"/> matches the given <see cref="Group"/>'s Path or Id.
    /// <summary>
    /// <remarks>
    /// Path comparison is case-insensitive.
    /// </remarks>
    /// <param name="other">The other identifier.</param>
    /// <returns>True if the GroupId match the Group, otherwise false.</returns>
    public bool Equals(Group other) =>
        other != null && Equals(other.FullPath, other.Id);

    /// <summary>
    /// Checks if this <see cref="GroupId"/> matches the given Path or Id.
    /// <summary>
    /// <remarks>
    /// Path comparison is case-insensitive.
    /// </remarks>
    /// <param name="otherPath">The other full path.</param>
    /// <param name="otherId">The other id.</param>
    /// <returns>True if the GroupId match the Group, otherwise false.</returns>
    public bool Equals(string otherPath, long otherId) =>
        _path != null ? StringComparer.OrdinalIgnoreCase.Equals(_path, otherPath) : _id == otherId;

    /// <inheritdoc cref="Equals(GroupId)"/>
    public static bool operator ==(GroupId a, GroupId b) => a.Equals(b);

    /// <inheritdoc cref="Equals(GroupId)"/>
    public static bool operator !=(GroupId a, GroupId b) => !a.Equals(b);

    /// <inheritdoc cref="Equals(Group)"/>
    public static bool operator ==(GroupId a, Group b) => a.Equals(b);

    /// <inheritdoc cref="Equals(Group)"/>
    public static bool operator !=(GroupId a, Group b) => !a.Equals(b);

    /// <inheritdoc cref="Equals(Group)"/>
    public static bool operator ==(Group a, GroupId b) => b.Equals(a);

    /// <inheritdoc cref="Equals(Group)"/>
    public static bool operator !=(Group a, GroupId b) => !b.Equals(a);
}
