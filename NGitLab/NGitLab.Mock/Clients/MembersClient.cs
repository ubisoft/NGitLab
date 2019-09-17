using System;
using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class MembersClient : ClientBase, IMembersClient
    {
        public MembersClient(ClientContext context)
            : base(context)
        {
        }

        public Membership AddMemberToProject(string projectId, ProjectMemberCreate user)
        {
            throw new NotImplementedException();
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
