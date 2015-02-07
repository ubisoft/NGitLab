using System.Collections.Generic;
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

        public IEnumerable<Project> Accessible
        {
            get
            {
                return _api.Get().GetAll<Project>(Project.Url);
            }
        }

        public IEnumerable<Project> Owned
        {
            get
            {
                return _api.Get().GetAll<Project>(Project.Url + "/owned");
            }
        }

        public IEnumerable<Project> All
        {
            get
            {
                return _api.Get().GetAll<Project>(Project.Url + "/all");
            }
        }

        public Project this[int id]
        {
            get
            {
                return _api.Get().To<Project>(Project.Url + "/" + id);
            }
        }

        public Project Create(ProjectCreate project)
        {
            return _api.Post().With(project).To<Project>(Project.Url);
        }

        public bool Delete(int id)
        {
            return _api.Delete().To<bool>(Project.Url + "/" + id);
        }
    }
}