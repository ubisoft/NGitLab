using System;
using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    /// <summary>
    /// https://github.com/gitlabhq/gitlabhq/blob/master/doc/api/members.md
    /// </summary>
    public interface IMembersClient
    {
        IEnumerable<Membership> OfProject(string projectId);

        IEnumerable<Membership> OfProject(string projectId, bool includeInheritedMembers);

        [Obsolete("Use OfGroup")]
        IEnumerable<Membership> OfNamespace(string groupId);

        IEnumerable<Membership> OfGroup(string groupId);

        IEnumerable<Membership> OfGroup(string groupId, bool includeInheritedMembers);

        Membership GetMemberOfGroup(string groupId, string userId);

        Membership GetMemberOfProject(string projectId, string userId);

        Membership GetMemberOfProject(string projectId, string userId, bool includeInheritedMembers);

        Membership AddMemberToProject(string projectId, ProjectMemberCreate user);

        Membership UpdateMemberOfProject(string projectId, ProjectMemberUpdate user);

        Membership AddMemberToGroup(string groupId, GroupMemberCreate user);

        Membership UpdateMemberOfGroup(string groupId, GroupMemberUpdate user);
    }
}
