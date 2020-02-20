using System;
using System.Collections.Generic;
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
            var project = GetProject(projectId, ProjectPermission.Edit);
            var user = Server.Users.GetById(projectMemberCreate.UserId);

            var existingPermission = project.GetEffectivePermissions().GetEffectivePermission(user);
            if (existingPermission != null)
            {
                if (existingPermission.AccessLevel > projectMemberCreate.AccessLevel)
                {
                    throw new GitLabException($"{{\"access_level\":[\"should be greater than or equal to Owner inherited membership from group Runners\"]}}. Original call: Post https://gitlab.example.com/api/v4/projects/{project.Id}/members. With data {{\"user_id\":\"{user.Id}\",\"access_level\":{(int)projectMemberCreate.AccessLevel}}}")
                    {
                        StatusCode = HttpStatusCode.BadRequest
                    };
                }

                if (existingPermission.AccessLevel == projectMemberCreate.AccessLevel)
                {
                    throw new GitLabException { StatusCode = HttpStatusCode.Conflict };
                }
            }

            var permission = new Permission(user, projectMemberCreate.AccessLevel);
            project.Permissions.Add(permission);

            return project.GetEffectivePermissions().GetEffectivePermission(user).ToMembershipClient();
        }

        public Membership GetMemberOfGroup(string groupId, string userId)
        {
            throw new NotImplementedException();
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
            var project = GetProject(projectId, ProjectPermission.View);
            var members = project.GetEffectivePermissions().Permissions;
            return members.Select(member => member.ToMembershipClient());
        }
    }
}
