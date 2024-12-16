namespace NGitLab.Models;

/// <summary>
/// The access levels are defined in the GitLab::Access module. Currently, these levels are recognized:
/// </summary>
/// <remarks>https://github.com/gitlabhq/gitlabhq/blob/master/doc/api/members.md</remarks>
public enum AccessLevel
{
    NoAccess = 0,
    Guest = 10,
    Reporter = 20,
    Developer = 30,
    Maintainer = 40,
    /// <summary>
    /// Only valid for groups.
    /// </summary>
    Owner = 50,
    Admin = 60,
}
