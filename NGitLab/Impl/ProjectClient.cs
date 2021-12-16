using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Extensions;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class ProjectClient : IProjectClient
    {
        private readonly API _api;

        public ProjectClient(API api)
        {
            _api = api;
        }

        public IEnumerable<Project> Accessible => _api.Get().GetAll<Project>(Utils.AddOrderBy(Project.Url + "?membership=true"));

        public IEnumerable<Project> Owned => _api.Get().GetAll<Project>(Utils.AddOrderBy(Project.Url + "?owned=true"));

        public IEnumerable<Project> Visible => _api.Get().GetAll<Project>(Utils.AddOrderBy(Project.Url));

        public Project this[int id] => GetById(id, new SingleProjectQuery());

        public Project Create(ProjectCreate project) => _api.Post().With(project).To<Project>(Project.Url);

        public Project this[string fullName] => _api.Get().To<Project>(Project.Url + "/" + WebUtility.UrlEncode(fullName));

        public void Delete(int id) => _api.Delete().Execute(Project.Url + "/" + id.ToString(CultureInfo.InvariantCulture));

        public void Archive(int id) => _api.Post().Execute(Project.Url + "/" + id.ToString(CultureInfo.InvariantCulture) + "/archive");

        public void Unarchive(int id) => _api.Post().Execute(Project.Url + "/" + id.ToString(CultureInfo.InvariantCulture) + "/unarchive");

        private static bool SupportKeysetPagination(ProjectQuery query)
        {
            return string.IsNullOrEmpty(query.Search);
        }

        private static string CreateGetUrl(ProjectQuery query)
        {
            var url = Project.Url;

            if (query.UserId.HasValue)
            {
                url = $"/users/{query.UserId.Value.ToStringInvariant()}/projects";
            }

            if (query.LastActivityAfter.HasValue)
            {
                url = Utils.AddParameter(url, "last_activity_after", query.LastActivityAfter.Value.UtcDateTime.ToString("O"));
            }

            switch (query.Scope)
            {
                case ProjectQueryScope.Accessible:
                    url = Utils.AddParameter(url, "membership", value: true);
                    break;
                case ProjectQueryScope.Owned:
                    url = Utils.AddParameter(url, "owned", value: true);
                    break;
#pragma warning disable 618 // Obsolete
                case ProjectQueryScope.Visible:
#pragma warning restore 618
                case ProjectQueryScope.All:
                    // This is the default, it returns all visible projects.
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            url = Utils.AddParameter(url, "archived", query.Archived);
            url = Utils.AddOrderBy(url, query.OrderBy, supportKeysetPagination: SupportKeysetPagination(query));
            url = Utils.AddParameter(url, "search", query.Search);
            url = Utils.AddParameter(url, "simple", query.Simple);
            url = Utils.AddParameter(url, "statistics", query.Statistics);
            url = Utils.AddParameter(url, "per_page", query.PerPage);

            if (query.Ascending == true)
            {
                url = Utils.AddParameter(url, "sort", "asc");
            }

            if (query.Visibility.HasValue)
            {
                url = Utils.AddParameter(url, "visibility", query.Visibility.ToString().ToLower());
            }

            if (query.MinAccessLevel != null)
            {
                url = Utils.AddParameter(url, "min_access_level", (int)query.MinAccessLevel.Value);
            }

            return url;
        }

        public IEnumerable<Project> Get(ProjectQuery query)
        {
            var url = CreateGetUrl(query);
            return _api.Get().GetAll<Project>(url);
        }

        public GitLabCollectionResponse<Project> GetAsync(ProjectQuery query)
        {
            var url = CreateGetUrl(query);
            return _api.Get().GetAllAsync<Project>(url);
        }

        public Project GetById(int id, SingleProjectQuery query)
        {
            var url = Project.Url + "/" + id.ToStringInvariant();
            url = Utils.AddParameter(url, "statistics", query.Statistics);

            return _api.Get().To<Project>(url);
        }

        public async Task<Project> GetByIdAsync(int id, SingleProjectQuery query, CancellationToken cancellationToken = default)
        {
            var url = Project.Url + "/" + id.ToStringInvariant();
            url = Utils.AddParameter(url, "statistics", query.Statistics);

            return await _api.Get().ToAsync<Project>(url, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Project> GetByNamespacedPathAsync(string path, SingleProjectQuery query = null, CancellationToken cancellationToken = default)
        {
            var url = Project.Url + "/" + WebUtility.UrlEncode(path);
            url = Utils.AddParameter(url, "statistics", query?.Statistics);

            return await _api.Get().ToAsync<Project>(url, cancellationToken).ConfigureAwait(false);
        }

        public Project Fork(string id, ForkProject forkProject)
        {
            return _api.Post().With(forkProject).To<Project>(Project.Url + "/" + id + "/fork");
        }

        public Task<Project> ForkAsync(string id, ForkProject forkProject, CancellationToken cancellationToken = default)
        {
            return _api.Post().With(forkProject).ToAsync<Project>(Project.Url + "/" + id + "/fork", cancellationToken);
        }

        public IEnumerable<Project> GetForks(string id, ForkedProjectQuery query)
        {
            var url = CreateGetForksUrl(id, query);
            return _api.Get().GetAll<Project>(url);
        }

        public GitLabCollectionResponse<Project> GetForksAsync(string id, ForkedProjectQuery query)
        {
            var url = CreateGetForksUrl(id, query);
            return _api.Get().GetAllAsync<Project>(url);
        }

        private static string CreateGetForksUrl(string id, ForkedProjectQuery query)
        {
            var url = Project.Url + "/" + id + "/forks";

            if (query != null)
            {
                url = Utils.AddParameter(url, "owned", query.Owned);
                url = Utils.AddParameter(url, "archived", query.Archived);
                url = Utils.AddParameter(url, "membership", query.Membership);
                url = Utils.AddOrderBy(url, query.OrderBy);
                url = Utils.AddParameter(url, "search", query.Search);
                url = Utils.AddParameter(url, "simple", query.Simple);
                url = Utils.AddParameter(url, "statistics", query.Statistics);
                url = Utils.AddParameter(url, "per_page", query.PerPage);

                if (query.Visibility.HasValue)
                {
                    url = Utils.AddParameter(url, "visibility", query.Visibility.ToString().ToLowerInvariant());
                }

                if (query.MinAccessLevel != null)
                {
                    url = Utils.AddParameter(url, "min_access_level", (int)query.MinAccessLevel.Value);
                }
            }

            return url;
        }

        public Dictionary<string, double> GetLanguages(string id)
        {
            var languages = DoGetLanguages(id);

            // After upgrading from v 11.6.2-ee to v 11.10.4-ee, the project /languages endpoint takes time execute.
            // So now we wait for the languages to be returned with a max wait time of 10 s.
            // The waiting logic should be removed once GitLab fix the issue in a version > 11.10.4-ee.
            var started = DateTime.UtcNow;
            while (!languages.Any() && (DateTime.UtcNow - started) < TimeSpan.FromSeconds(10))
            {
                Thread.Sleep(1000);
                languages = DoGetLanguages(id);
            }

            return languages;
        }

        private Dictionary<string, double> DoGetLanguages(string id)
        {
            return _api.Get().To<Dictionary<string, double>>(Project.Url + "/" + id + "/languages");
        }

        public Project Update(string id, ProjectUpdate projectUpdate)
        {
            return _api.Put().With(projectUpdate).To<Project>(Project.Url + "/" + Uri.EscapeDataString(id));
        }

        public UploadedProjectFile UploadFile(string id, FormDataContent data)
            => _api.Post().With(data).To<UploadedProjectFile>($"{Project.Url}/{Uri.EscapeDataString(id)}/uploads");
    }
}
