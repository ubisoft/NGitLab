using NGitLab.Models;
using System;
using System.Collections.Generic;
using System.Net;

namespace NGitLab.Impl
{
    public class MembersClient : IMembersClient
    {
        private readonly API _api;

        public MembersClient(API api)
        {
            _api = api;
        }

        private IEnumerable<Membership> GetAll(string url) => _api.Get().GetAll<Membership>(url + "/members");

        public IEnumerable<Membership> OfProject(string projectId)
        {
            return GetAll(Project.Url + "/" + WebUtility.UrlEncode(projectId));
        }

        [Obsolete("Use OfGroup")]
        public IEnumerable<Membership> OfNamespace(string groupId)
        {
            return OfGroup(groupId);
        }

        public IEnumerable<Membership> OfGroup(string groupId)
        {
            return GetAll(GroupsClient.Url + "/" + WebUtility.UrlEncode(groupId));
        }

        public Membership GetMemberOfGroup(string groupId, string userId)
        {
            return _api.Get().To<Membership>(GroupsClient.Url + "/" + WebUtility.UrlEncode(groupId) + "/members/" + WebUtility.UrlEncode(userId));
        }

        public Membership AddMemberToProject(string projectId, ProjectMemberCreate user)
        {
            return _api.Post().With(user).To<Membership>(Project.Url + "/" + WebUtility.UrlEncode(projectId) + "/members");
        }
    }
}