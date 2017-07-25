using System.Collections.Generic;
using System.Security.Policy;
using System.Web;
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

        public IEnumerable<Project> Accessible => _api.Get().GetAll<Project>(Project.Url);

        public IEnumerable<Project> Owned => _api.Get().GetAll<Project>(Project.Url + "/owned");

        public IEnumerable<Project> Visible => _api.Get().GetAll<Project>(Project.Url + "/visible");

        public IEnumerable<Project> All => _api.Get().GetAll<Project>(Project.Url + "/all");

        public Project this[int id] => _api.Get().To<Project>(Project.Url + "/" + id);

        public Project Create(ProjectCreate project) => _api.Post().With(project).To<Project>(Project.Url);

        public Project this[string fullName] => _api.Get().To<Project>(Project.Url + "/" + System.Web.HttpUtility.UrlEncode(fullName));
        
        public bool Delete(int id) => _api.Delete().To<bool>(Project.Url + "/" + id);

        public IEnumerable<Project> Get(ProjectQuery query)
        {
            string url = Project.Url;
            if (query.Scope != ProjectQueryScope.Accessible)
            {
                url += "/" + query.Scope.ToString().ToLower();
            }

            url = AddParameter(url, "archived", query.Archived);
            url = AddParameter(url, "order_by", query.OrderBy);
            url = AddParameter(url, "search", query.Search);
            url = AddParameter(url, "simple", query.Simple);

            if (query.Ascending == true)
            {
                url = AddParameter(url, "sort", "asc");
            }

            if (query.Visibility.HasValue)
            {
                url = AddParameter(url, "visibility", query.Visibility.ToString().ToLower());
            }

            return _api.Get().GetAll<Project>(url);
        }

        private static string AddParameter<T>(string url, string parameterName, T value)
        {
            if (Equals(value, null))
            {
                return url;
            }

            string @operator = !url.Contains("?") ? "?" : "&";
            var formattedValue = HttpUtility.UrlEncode(value.ToString());
            var parameter = $"{@operator}{parameterName}={formattedValue}";
            return url + parameter;
        }
    }
}