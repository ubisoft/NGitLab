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
                return _api.Retrieve().GetAll<Project>(Project.Url);
            }
        }

        public IEnumerable<Project> Owned
        {
            get
            {
                return _api.Retrieve().GetAll<Project>(Project.Url + "/owned");
            }
        }

        public IEnumerable<Project> All
        {
            get
            {
                return _api.Retrieve().GetAll<Project>(Project.Url + "/all");
            }
        }

        public Project this[int id]
        {
            get
            {
                return _api.Retrieve().To<Project>(Project.Url + "/" + id);
            }
        }
    }
}