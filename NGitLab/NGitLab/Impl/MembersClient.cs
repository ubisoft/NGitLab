using System;
using System.Collections.Generic;
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

        private IEnumerable<Membership> GetAll(string url) => _api.Get().GetAll<Membership>(url + "/members");

        public IEnumerable<Membership> OfProject(string projectId)
        {
            return GetAll(Project.Url + "/" + System.Web.HttpUtility.UrlEncode(projectId));
        }

        [Obsolete("Use OfGroup")]
        public IEnumerable<Membership> OfNamespace(string groupId)
        {
            return OfGroup(groupId);
        }

        public IEnumerable<Membership> OfGroup(string groupId)
        {
            return GetAll(GroupsClient.Url + "/" + System.Web.HttpUtility.UrlEncode(groupId));
        }

        public Membership AddMemberToProject(string projectId, ProjectMemberCreate user)
        {
            return _api.Post().With(user).To<Membership>(Project.Url + "/" + System.Web.HttpUtility.UrlEncode(projectId) + "/members");
        }
    }
}