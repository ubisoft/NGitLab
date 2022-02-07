using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
            var url = CreateGetUrl(query);
            return _api.Get().GetAll<Group>(url);
        }

        public GitLabCollectionResponse<Group> GetAsync(GroupQuery query)
        {
            var url = CreateGetUrl(query);
            return _api.Get().GetAllAsync<Group>(url);
        }

        private static string CreateGetUrl(GroupQuery query)
        {
            var url = Group.Url;

            if (query.SkipGroups != null && query.SkipGroups.Any())
            {
                foreach (var skipGroup in query.SkipGroups)
                {
                    url = Utils.AddParameter(url, "skip_groups[]", skipGroup);
                }
            }

            if (query.AllAvailable != null)
            {
                url = Utils.AddParameter(url, "all_available", query.AllAvailable);
            }

            if (!string.IsNullOrEmpty(query.Search))
            {
                url = Utils.AddParameter(url, "search", query.Search);
            }

            url = Utils.AddOrderBy(url, query.OrderBy);

            if (query.Sort != null)
            {
                url = Utils.AddParameter(url, "sort", query.Sort);
            }

            if (query.Statistics != null)
            {
                url = Utils.AddParameter(url, "statistics", query.Statistics);
            }

            if (query.WithCustomAttributes != null)
            {
                url = Utils.AddParameter(url, "with_custom_attributes", query.WithCustomAttributes);
            }

            if (query.Owned != null)
            {
                url = Utils.AddParameter(url, "owned", query.Owned);
            }

            if (query.MinAccessLevel != null)
            {
                url = Utils.AddParameter(url, "min_access_level", (int)query.MinAccessLevel);
            }

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

        public IEnumerable<Project> SearchProjects(int groupId, string search)
        {
            var url = Url + "/" + Uri.EscapeDataString(groupId.ToString(CultureInfo.InvariantCulture)) + "/projects";
            url = Utils.AddParameter(url, "search", search);
            url = Utils.AddOrderBy(url);
            return _api.Get().GetAll<Project>(url);
        }

        // implementation according to https://docs.gitlab.com/ee/api/search.html#scope-projects-1
        public IEnumerable<Project> SearchProjects(SearchProjectQuery searchProjectQuery)
        {
            var url = Url + "/" + Uri.EscapeDataString(searchProjectQuery.GroupId.ToString(CultureInfo.InvariantCulture)) + "/search";
            url = Utils.AddParameter(url, "scope", searchProjectQuery.Scope.ToString().ToLowerInvariant());
            url = Utils.AddParameter(url, "search", searchProjectQuery.Search);
            url = Utils.AddOrderBy(url);
            return _api.Get().GetAll<Project>(url);
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
