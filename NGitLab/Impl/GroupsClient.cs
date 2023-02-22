using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
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

        public IEnumerable<Group> Accessible => _api.Get().GetAll<Group>(Utils.AddOrderBy(Url));

        public IEnumerable<Group> Get(GroupQuery query)
        {
            var url = QueryStringHelper.BuildAndAppendQueryString(Group.Url, query);
            return _api.Get().GetAll<Group>(url);
        }

        public GitLabCollectionResponse<Group> GetAsync(GroupQuery query)
        {
            var url = QueryStringHelper.BuildAndAppendQueryString(Group.Url, query);
            return _api.Get().GetAllAsync<Group>(url);
        }

        private static string CreateSubgroupGetUrl(SubgroupQuery query, string id)
        {
            var url = QueryStringHelper.BuildAndAppendQueryString(Group.Url + "/" + id + "/subgroups", query);
            return url;
        }

        public IEnumerable<Group> Search(string search)
        {
            return _api.Get().GetAll<Group>(Utils.AddOrderBy(Url + $"?search={Uri.EscapeDataString(search)}"));
        }

        public GitLabCollectionResponse<Group> SearchAsync(string search)
        {
            return _api.Get().GetAllAsync<Group>(Utils.AddOrderBy(Url + $"?search={Uri.EscapeDataString(search)}"));
        }

        public Group this[int id] => _api.Get().To<Group>(Url + "/" + Uri.EscapeDataString(id.ToString(CultureInfo.InvariantCulture)));

        public Task<Group> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return _api.Get().ToAsync<Group>(Url + "/" + Uri.EscapeDataString(id.ToString(CultureInfo.InvariantCulture)), cancellationToken);
        }

        public Group this[string fullPath] => _api.Get().To<Group>(Url + "/" + Uri.EscapeDataString(fullPath));

        public Task<Group> GetByFullPathAsync(string fullPath, CancellationToken cancellationToken = default)
        {
            return _api.Get().ToAsync<Group>(Url + "/" + Uri.EscapeDataString(fullPath), cancellationToken);
        }

        public GitLabCollectionResponse<Group> GetSubgroupsByIdAsync(int id, SubgroupQuery query = null)
        {
            var url = CreateSubgroupGetUrl(query, Uri.EscapeDataString(id.ToString(CultureInfo.InvariantCulture)));
            return _api.Get().GetAllAsync<Group>(url);
        }

        public GitLabCollectionResponse<Group> GetSubgroupsByFullPathAsync(string fullPath, SubgroupQuery query = null)
        {
            var url = CreateSubgroupGetUrl(query, Uri.EscapeDataString(fullPath));
            return _api.Get().GetAllAsync<Group>(url);
        }

        public IEnumerable<Project> SearchProjects(int groupId, string search)
        {
            return GetProjectsAsync(groupId, new GroupProjectsQuery
            {
                Search = search,
            });
        }

        public GitLabCollectionResponse<Project> GetProjectsAsync(int groupId, GroupProjectsQuery query)
        {
            var url = QueryStringHelper.BuildAndAppendQueryString(Url + "/" + Uri.EscapeDataString(groupId.ToString(CultureInfo.InvariantCulture)) + "/projects", query);
            return _api.Get().GetAllAsync<Project>(url);
        }

        public Group Create(GroupCreate group) => _api.Post().With(group).To<Group>(Url);

        public Task<Group> CreateAsync(GroupCreate group, CancellationToken cancellationToken = default)
        {
            return _api.Post().With(group).ToAsync<Group>(Url, cancellationToken);
        }

        public void Delete(int id)
        {
            _api.Delete().Execute(Url + "/" + Uri.EscapeDataString(id.ToString(CultureInfo.InvariantCulture)));
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            return _api.Delete().ExecuteAsync(Url + "/" + Uri.EscapeDataString(id.ToString(CultureInfo.InvariantCulture)), cancellationToken);
        }

        public Group Update(int id, GroupUpdate groupUpdate) => _api.Put().With(groupUpdate).To<Group>(Url + "/" + Uri.EscapeDataString(id.ToString(CultureInfo.InvariantCulture)));

        public Task<Group> UpdateAsync(int id, GroupUpdate groupUpdate, CancellationToken cancellationToken = default)
        {
            return _api.Put().With(groupUpdate).ToAsync<Group>(Url + "/" + Uri.EscapeDataString(id.ToString(CultureInfo.InvariantCulture)), cancellationToken);
        }

        public void Restore(int id) => _api.Post().Execute(Url + "/" + Uri.EscapeDataString(id.ToString(CultureInfo.InvariantCulture)) + "/restore");
    }
}
