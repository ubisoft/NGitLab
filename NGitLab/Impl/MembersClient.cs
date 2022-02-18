using System;
using System.Collections.Generic;
using System.Net;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class MembersClient : IMembersClient
    {
        private readonly API _api;

        public MembersClient(API api)
        {
            _api = api;
        }

        private IEnumerable<Membership> GetAll(string url, bool includeInheritedMembers)
        {
            url += "/members";
            if (includeInheritedMembers)
            {
                url += "/all";
            }

            return _api.Get().GetAll<Membership>(url);
        }

        public IEnumerable<Membership> OfProject(string projectId)
        {
            return OfProject(projectId, includeInheritedMembers: false);
        }

        public IEnumerable<Membership> OfProject(string projectId, bool includeInheritedMembers)
        {
            return GetAll(Project.Url + "/" + WebUtility.UrlEncode(projectId), includeInheritedMembers);
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
            return GetAll(GroupsClient.Url + "/" + WebUtility.UrlEncode(groupId), includeInheritedMembers);
        }

        public Membership GetMemberOfGroup(string groupId, string userId)
        {
            return _api.Get().To<Membership>(GroupsClient.Url + "/" + WebUtility.UrlEncode(groupId) + "/members/" + WebUtility.UrlEncode(userId));
        }

        public Membership GetMemberOfProject(string projectId, string userId)
        {
            return GetMemberOfProject(projectId, userId, includeInheritedMembers: false);
        }

        public Membership GetMemberOfProject(string projectId, string userId, bool includeInheritedMembers)
        {
            var url = $"{Project.Url}/{WebUtility.UrlEncode(projectId)}/members/{(includeInheritedMembers ? "all/" : string.Empty)}{WebUtility.UrlEncode(userId)}";

            return _api.Get().To<Membership>(url);
        }

        public Membership AddMemberToProject(string projectId, ProjectMemberCreate user)
        {
            return _api.Post().With(user).To<Membership>(Project.Url + "/" + WebUtility.UrlEncode(projectId) + "/members");
        }

        public Membership UpdateMemberOfProject(string projectId, ProjectMemberUpdate user)
        {
            return _api.Put().With(user).To<Membership>(Project.Url + "/" + WebUtility.UrlEncode(projectId) + "/members/" + WebUtility.UrlEncode(user.UserId));
        }

        public Membership AddMemberToGroup(string groupId, GroupMemberCreate user)
        {
            return _api.Post().With(user).To<Membership>(Group.Url + "/" + WebUtility.UrlEncode(groupId) + "/members");
        }

        public Membership UpdateMemberOfGroup(string groupId, GroupMemberUpdate user)
        {
            return _api.Put().With(user).To<Membership>(Group.Url + "/" + WebUtility.UrlEncode(groupId) + "/members/" + WebUtility.UrlEncode(user.UserId));
        }
    }
}
