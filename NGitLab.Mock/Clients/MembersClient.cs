using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class MembersClient : ClientBase, IMembersClient
    {
        public MembersClient(ClientContext context)
            : base(context)
        {
        }

        public Membership AddMemberToProject(string projectId, ProjectMemberCreate projectMemberCreate)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(projectId, ProjectPermission.Edit);
                var user = Server.Users.GetById(projectMemberCreate.UserId);

                CheckUserPermissionOfProject(projectMemberCreate.AccessLevel, user, project);

                var permission = new Permission(user, projectMemberCreate.AccessLevel);
                project.Permissions.Add(permission);

                return project.GetEffectivePermissions().GetEffectivePermission(user).ToMembershipClient();
            }
        }

        public Membership UpdateMemberOfProject(string projectId, ProjectMemberUpdate projectMemberUpdate)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(projectId, ProjectPermission.Edit);
                var user = Server.Users.GetById(projectMemberUpdate.UserId);

                CheckUserPermissionOfProject(projectMemberUpdate.AccessLevel, user, project);
                return project.GetEffectivePermissions().GetEffectivePermission(user).ToMembershipClient();
            }
        }

        public Membership AddMemberToGroup(string groupId, GroupMemberCreate groupMemberCreate)
        {
            using (Context.BeginOperationScope())
            {
                var @group = GetGroup(groupId, GroupPermission.Edit);
                var user = Server.Users.GetById(groupMemberCreate.UserId);

                CheckUserPermissionOfGroup(groupMemberCreate.AccessLevel, user, @group);

                var permission = new Permission(user, groupMemberCreate.AccessLevel);
                @group.Permissions.Add(permission);

                return @group.GetEffectivePermissions().GetEffectivePermission(user).ToMembershipClient();
            }
        }

        public Membership UpdateMemberOfGroup(string groupId, GroupMemberUpdate groupMemberUpdate)
        {
            using (Context.BeginOperationScope())
            {
                var @group = GetGroup(groupId, GroupPermission.Edit);
                var user = Server.Users.GetById(groupMemberUpdate.UserId);

                CheckUserPermissionOfGroup(groupMemberUpdate.AccessLevel, user, @group);
                return @group.GetEffectivePermissions().GetEffectivePermission(user).ToMembershipClient();
            }
        }

        public Membership GetMemberOfGroup(string groupId, string userId)
        {
            return OfGroup(groupId, includeInheritedMembers: false)
                .FirstOrDefault(u => string.Equals(u.Id.ToString(CultureInfo.InvariantCulture), userId, StringComparison.Ordinal));
        }

        public Membership GetMemberOfProject(string projectId, string userId)
        {
            return OfProject(projectId, includeInheritedMembers: false)
                .FirstOrDefault(u => string.Equals(u.Id.ToString(CultureInfo.InvariantCulture), userId, StringComparison.Ordinal));
        }

        public Membership GetMemberOfProject(string projectId, string userId, bool includeInheritedMembers)
        {
            return OfProject(projectId, includeInheritedMembers)
               .FirstOrDefault(u => string.Equals(u.Id.ToString(CultureInfo.InvariantCulture), userId, StringComparison.Ordinal));
        }

        public IEnumerable<Membership> OfGroup(string groupId)
        {
            return OfGroup(groupId, includeInheritedMembers: false);
        }

        public IEnumerable<Membership> OfGroup(string groupId, bool includeInheritedMembers)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Membership> OfNamespace(string groupId)
        {
            return OfGroup(groupId);
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
                var members = project.GetEffectivePermissions().Permissions;
                return members.Select(member => member.ToMembershipClient());
            }
        }

        private static void CheckUserPermissionOfProject(AccessLevel accessLevel, User user, Project project)
        {
            var existingPermission = project.GetEffectivePermissions().GetEffectivePermission(user);
            if (existingPermission != null)
            {
                if (existingPermission.AccessLevel > accessLevel)
                {
                    throw new GitLabException($"{{\"access_level\":[\"should be greater than or equal to Owner inherited membership from group Runners\"]}}.")
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                    };
                }

                if (existingPermission.AccessLevel == accessLevel)
                {
                    throw new GitLabException { StatusCode = HttpStatusCode.Conflict };
                }
            }
        }

        private static void CheckUserPermissionOfGroup(AccessLevel accessLevel, User user, Group @group)
        {
            var existingPermission = @group.GetEffectivePermissions().GetEffectivePermission(user);
            if (existingPermission != null)
            {
                if (existingPermission.AccessLevel > accessLevel)
                {
                    throw new GitLabException($"{{\"access_level\":[\"should be greater than or equal to Owner inherited membership from group Runners\"]}}.")
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                    };
                }

                if (existingPermission.AccessLevel == accessLevel)
                {
                    throw new GitLabException { StatusCode = HttpStatusCode.Conflict };
                }
            }
        }
    }
}
