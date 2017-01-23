using System.Collections.Generic;
using System.Security.Policy;
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
    }
}