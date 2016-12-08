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

        public IEnumerable<Membership> OfNamespace(string groupId)
        {
            return GetAll(Namespace.Url + "/" + System.Web.HttpUtility.UrlEncode(groupId));
        }
    }
}