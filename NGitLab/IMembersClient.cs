using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab;

/// <summary>
/// https://github.com/gitlabhq/gitlabhq/blob/master/doc/api/members.md
/// </summary>
public interface IMembersClient
{
    IEnumerable<Membership> OfProject(string projectId);

    IEnumerable<Membership> OfProject(string projectId, bool includeInheritedMembers);

    IEnumerable<Membership> OfProject(string projectId, bool includeInheritedMembers, MemberQuery query);

    GitLabCollectionResponse<Membership> OfProjectAsync(ProjectId projectId, bool includeInheritedMembers);

    GitLabCollectionResponse<Membership> OfProjectAsync(ProjectId projectId, bool includeInheritedMembers = false, MemberQuery query = null);

    Membership GetMemberOfProject(string projectId, string userId);

    Membership GetMemberOfProject(string projectId, string userId, bool includeInheritedMembers);

    Task<Membership> GetMemberOfProjectAsync(ProjectId projectId, long userId, bool includeInheritedMembers = false, CancellationToken cancellationToken = default);

    Membership AddMemberToProject(string projectId, ProjectMemberCreate user);

    Task<Membership> AddMemberToProjectAsync(ProjectId projectId, ProjectMemberCreate user, CancellationToken cancellationToken = default);

    Membership UpdateMemberOfProject(string projectId, ProjectMemberUpdate user);

    Task<Membership> UpdateMemberOfProjectAsync(ProjectId projectId, ProjectMemberUpdate user, CancellationToken cancellationToken = default);

    Task RemoveMemberFromProjectAsync(ProjectId projectId, long userId, CancellationToken cancellationToken = default);

    IEnumerable<Membership> OfGroup(string groupId);

    IEnumerable<Membership> OfGroup(string groupId, bool includeInheritedMembers);

    IEnumerable<Membership> OfGroup(string groupId, bool includeInheritedMembers, MemberQuery query);

    GitLabCollectionResponse<Membership> OfGroupAsync(GroupId groupId, bool includeInheritedMembers);

    GitLabCollectionResponse<Membership> OfGroupAsync(GroupId groupId, bool includeInheritedMembers = false, MemberQuery query = null);

    Membership GetMemberOfGroup(string groupId, string userId);

    Membership GetMemberOfGroup(string groupId, string userId, bool includeInheritedMembers);

    Task<Membership> GetMemberOfGroupAsync(GroupId groupId, long userId, bool includeInheritedMembers = false, CancellationToken cancellationToken = default);

    Membership AddMemberToGroup(string groupId, GroupMemberCreate user);

    Task<Membership> AddMemberToGroupAsync(GroupId groupId, GroupMemberCreate user, CancellationToken cancellationToken = default);

    Membership UpdateMemberOfGroup(string groupId, GroupMemberUpdate user);

    Task<Membership> UpdateMemberOfGroupAsync(GroupId groupId, GroupMemberUpdate user, CancellationToken cancellationToken = default);

    Task RemoveMemberFromGroupAsync(GroupId groupId, long userId, CancellationToken cancellationToken = default);
}
