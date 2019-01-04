using NGitLab.Models;
using System;
using System.Collections.Generic;
using System.Net;

namespace NGitLab.Impl
{
    public class ProjectClient : IProjectClient
    {
        private readonly API _api;

        public ProjectClient(API api)
        {
            _api = api;
        }

        public IEnumerable<Project> Accessible => _api.Get().GetAll<Project>(Project.Url + "?membership=true");

        public IEnumerable<Project> Owned => _api.Get().GetAll<Project>(Project.Url + "?owned=true");

        public IEnumerable<Project> Visible => _api.Get().GetAll<Project>(Project.Url);

        public Project this[int id] => GetById(id, new SingleProjectQuery());

        public Project Create(ProjectCreate project) => _api.Post().With(project).To<Project>(Project.Url);

        public Project this[string fullName] => _api.Get().To<Project>(Project.Url + "/" + WebUtility.UrlEncode(fullName));

        public void Delete(int id) => _api.Delete().Execute(Project.Url + "/" + id);

        public IEnumerable<Project> Get(ProjectQuery query)
        {
            var url = Project.Url;

            if (query.UserId.HasValue)
            {
                url = $"/users/{query.UserId.Value}/projects";
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
            url = Utils.AddParameter(url, "order_by", query.OrderBy);
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

            return _api.Get().GetAll<Project>(url);
        }

        public Project GetById(int id, SingleProjectQuery query)
        {
            var url = Project.Url + "/" + id;
            url = Utils.AddParameter(url, "statistics", query.Statistics);

            return _api.Get().To<Project>(url);
        }

        public Project Fork(string id, ForkProject forkProject)
        {
            return _api.Post().With(forkProject).To<Project>(Project.Url + "/" + id + "/fork");
        }
    }
}
