using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Extensions;
using NGitLab.Mock.Internals;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class MembersClient : ClientBase, IMembersClient
{
    public MembersClient(ClientContext context)
        : base(context)
    {
    }

    public IEnumerable<Membership> OfProject(string projectId)
    {
        return OfProject(projectId, includeInheritedMembers: false);
    }

    public IEnumerable<Membership> OfProject(string projectId, bool includeInheritedMembers)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(projectId, ProjectPermission.View);
            var members = project.GetEffectivePermissions(includeInheritedMembers).Permissions;
            return members.Select(member => member.ToMembershipClient());
        }
    }

    public GitLabCollectionResponse<Membership> OfProjectAsync(ProjectId projectId, bool includeInheritedMembers = false)
    {
        return GitLabCollectionResponse.Create(OfProject(projectId.ValueAsString(), includeInheritedMembers));
    }

    public Membership AddMemberToProject(string projectId, ProjectMemberCreate projectMemberCreate)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(projectId, ProjectPermission.Edit);
            var user = Server.Users.GetById(projectMemberCreate.UserId);

            if (project.Permissions.Any(p => p.User.Id == user.Id))
            {
                throw new GitLabException("Member already exists")
                {
                    // actual GitLab error
                    StatusCode = HttpStatusCode.Conflict,
                    ErrorMessage = "Member already exists",
                };
            }

            ValidateNewProjectPermission(projectMemberCreate.AccessLevel, user, project);

            project.Permissions.Add(new(user, projectMemberCreate.AccessLevel));

            return project.GetEffectivePermissions().GetEffectivePermission(user).ToMembershipClient();
        }
    }

    public async Task<Membership> AddMemberToProjectAsync(ProjectId projectId, ProjectMemberCreate user, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        return AddMemberToProject(projectId.ValueAsString(), user);
    }

    public Membership UpdateMemberOfProject(string projectId, ProjectMemberUpdate projectMemberUpdate)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(projectId, ProjectPermission.Edit);
            var user = Server.Users.GetById(projectMemberUpdate.UserId);

            var curPermission = project.Permissions.SingleOrDefault(p => p.User.Id == user.Id)
                ?? throw new GitLabNotFoundException();

            ValidateNewProjectPermission(projectMemberUpdate.AccessLevel, user, project);

            project.Permissions.Remove(curPermission);
            project.Permissions.Add(new(user, projectMemberUpdate.AccessLevel));

            return project.GetEffectivePermissions().GetEffectivePermission(user).ToMembershipClient();
        }
    }

    public async Task<Membership> UpdateMemberOfProjectAsync(ProjectId projectId, ProjectMemberUpdate projectMemberUpdate, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        return UpdateMemberOfProject(projectId.ValueAsString(), projectMemberUpdate);
    }

    public Membership GetMemberOfProject(string projectId, string userId)
    {
        return GetMemberOfProject(projectId, userId, includeInheritedMembers: false);
    }

    public Membership GetMemberOfProject(string projectId, string userId, bool includeInheritedMembers)
    {
        return OfProject(projectId, includeInheritedMembers)
           .FirstOrDefault(u => string.Equals(u.Id.ToStringInvariant(), userId, StringComparison.Ordinal))
           ?? throw new GitLabNotFoundException();
    }

    public async Task<Membership> GetMemberOfProjectAsync(ProjectId projectId, long userId, bool includeInheritedMembers = false, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        return GetMemberOfProject(projectId.ValueAsString(), userId.ToStringInvariant(), includeInheritedMembers);
    }

    public async Task RemoveMemberFromProjectAsync(ProjectId projectId, long userId, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        using (Context.BeginOperationScope())
        {
            var project = GetProject(projectId, ProjectPermission.Edit);

            var permission = project.Permissions.SingleOrDefault(p => p.User.Id == userId)
                ?? throw new GitLabNotFoundException();

            project.Permissions.Remove(permission);
        }
    }

    [Obsolete("Use OfGroup")]
    public IEnumerable<Membership> OfNamespace(string groupId)
    {
        return OfGroup(groupId);
    }

    public IEnumerable<Membership> OfGroup(string groupId)
    {
        return OfGroup(groupId, includeInheritedMembers: false);
    }

    public IEnumerable<Membership> OfGroup(string groupId, bool includeInheritedMembers)
    {
        using (Context.BeginOperationScope())
        {
            var group = GetGroup(groupId, GroupPermission.View);
            var members = group.GetEffectivePermissions(includeInheritedMembers).Permissions;
            return members.Select(member => member.ToMembershipClient());
        }
    }

    public GitLabCollectionResponse<Membership> OfGroupAsync(GroupId groupId, bool includeInheritedMembers = false)
    {
        return GitLabCollectionResponse.Create(OfGroup(groupId.ValueAsString(), includeInheritedMembers));
    }

    public Membership AddMemberToGroup(string groupId, GroupMemberCreate groupMemberCreate)
    {
        using (Context.BeginOperationScope())
        {
            var group = GetGroup(groupId, GroupPermission.Edit);
            var user = Server.Users.GetById(groupMemberCreate.UserId);

            if (group.Permissions.Any(p => p.User.Id == user.Id))
            {
                throw new GitLabException("Member already exists")
                {
                    // actual GitLab error
                    StatusCode = HttpStatusCode.Conflict,
                    ErrorMessage = "Member already exists",
                };
            }

            ValidateNewGroupPermission(groupMemberCreate.AccessLevel, user, group);

            group.Permissions.Add(new(user, groupMemberCreate.AccessLevel));

            return group.GetEffectivePermissions().GetEffectivePermission(user).ToMembershipClient();
        }
    }

    public async Task<Membership> AddMemberToGroupAsync(GroupId groupId, GroupMemberCreate user, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        return AddMemberToGroup(groupId.ValueAsString(), user);
    }

    public Membership UpdateMemberOfGroup(string groupId, GroupMemberUpdate groupMemberUpdate)
    {
        using (Context.BeginOperationScope())
        {
            var group = GetGroup(groupId, GroupPermission.Edit);
            var user = Server.Users.GetById(groupMemberUpdate.UserId);

            var curPermission = group.Permissions.SingleOrDefault(p => p.User.Id == user.Id)
                ?? throw new GitLabNotFoundException();

            ValidateNewGroupPermission(groupMemberUpdate.AccessLevel, user, group);

            group.Permissions.Remove(curPermission);
            group.Permissions.Add(new(user, groupMemberUpdate.AccessLevel));

            return group.GetEffectivePermissions().GetEffectivePermission(user).ToMembershipClient();
        }
    }

    public async Task<Membership> UpdateMemberOfGroupAsync(GroupId groupId, GroupMemberUpdate groupMemberUpdate, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        return UpdateMemberOfGroup(groupId.ValueAsString(), groupMemberUpdate);
    }

    public Membership GetMemberOfGroup(string groupId, string userId)
    {
        return GetMemberOfGroup(groupId, userId, false);
    }

    public Membership GetMemberOfGroup(string groupId, string userId, bool includeInheritedMembers)
    {
        return OfGroup(groupId, includeInheritedMembers: includeInheritedMembers)
            .FirstOrDefault(u => string.Equals(u.Id.ToStringInvariant(), userId, StringComparison.Ordinal))
            ?? throw new GitLabNotFoundException();
    }

    public async Task<Membership> GetMemberOfGroupAsync(GroupId groupId, long userId, bool includeInheritedMembers = false, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        return GetMemberOfGroup(groupId.ValueAsString(), userId.ToStringInvariant(), includeInheritedMembers);
    }

    public async Task RemoveMemberFromGroupAsync(GroupId groupId, long userId, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        using (Context.BeginOperationScope())
        {
            var group = GetGroup(groupId, GroupPermission.Edit);

            var permission = group.Permissions.SingleOrDefault(p => p.User.Id == userId)
                ?? throw new GitLabNotFoundException();

            group.Permissions.Remove(permission);
        }
    }

    /// <summary>
    /// Checks if the given permission can be applied to the project and throws an exception if it is invalid.
    /// </summary>
    /// <exception cref="GitLabException">The new access level is less than an inherited membership role.</exception>
    private static void ValidateNewProjectPermission(AccessLevel newAccessLevel, User user, Project project)
    {
        // Get the existing permission from the parent, if it exists...
        var permission = project.Group?.GetEffectivePermissions().GetEffectivePermission(user);
        if (permission?.AccessLevel > newAccessLevel)
        {
            throw new GitLabException("access_level should be greater than or equal to inherited membership.")
            {
                // actual GitLab error
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessage = $$"""{"access_level":["should be greater than or equal to {{permission.AccessLevel}} inherited membership from project {{project.PathWithNamespace}}"]}.""",
            };
        }
    }

    /// <summary>
    /// Checks if the given permission can be applied to the group and throws an exception if it is invalid.
    /// </summary>
    /// <exception cref="GitLabException">The new access level is less than an inherited membership role.</exception>
    private static void ValidateNewGroupPermission(AccessLevel newAccessLevel, User user, Group group)
    {
        // Get the existing permission from the parent, if it exists...
        var permission = group.Parent?.GetEffectivePermissions().GetEffectivePermission(user);
        if (permission?.AccessLevel > newAccessLevel)
        {
            throw new GitLabException("access_level should be greater than or equal to inherited membership.")
            {
                // actual GitLab error
                StatusCode = HttpStatusCode.BadRequest,
                ErrorMessage = $$"""{"access_level":["should be greater than or equal to {{permission.AccessLevel}} inherited membership from group {{group.PathWithNameSpace}}"]}.""",
            };
        }
    }
}
