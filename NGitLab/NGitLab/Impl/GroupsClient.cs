using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Impl
{
    /// <summary>
    /// https://docs.gitlab.com/ce/api/groups.html
    /// </summary>
    public class GroupsClient : IGroupsClient
    {
        private readonly API _api;

        public const string Url = "/groups";

        public GroupsClient(API api)
        {
            _api = api;
        }

        public IEnumerable<Group> Accessible => _api.Get().GetAll<Group>(Url);

        public IEnumerable<Group> Search(string search)
        {
            return _api.Get().GetAll<Group>(Url + $"?search={search}");
        }

        public Group this[int id] => _api.Get().To<Group>(Url + "/" + id);

        public IEnumerable<Project> SearchProjects(int groupId, string search)
        {
            return _api.Get().GetAll<Project>(Url + "/" + groupId + $"/projects?search={search}");
        }

        public Group Create(GroupCreate group) => _api.Post().With(group).To<Group>(Url);

        public void Delete(int id)
        {
            _api.Delete().Execute(Url + "/" + id);
        }
    }
}