using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class MembersClient : IMembersClient
    {
        private readonly API _api;
        private readonly string _url;

        public MembersClient(API api, string url)
        {
            _api = api;
            _url = url;
        }

        public static MembersClient OfProject(API api, string projectId)
        {
            return new MembersClient(api, Project.Url + "/" + projectId);
        }

        public static MembersClient OfNamespace(API api, string groupId)
        {
            return new MembersClient(api, Namespace.Url + "/" + groupId);
        }

        public IEnumerable<Membership> All => _api.Get().GetAll<Membership>(_url + "/members");
    }
}