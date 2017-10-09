using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl {
    public class ProjectClient : IProjectClient {
        readonly Api api;

        public ProjectClient(Api api) {
            this.api = api;
        }

        public IEnumerable<Project> Accessible() {
            return api.Get().GetAll<Project>(Project.Url);
        }
        
        public IEnumerable<Project> Owned() {
            return api.Get().GetAll<Project>(Project.Url + "/?owned=true");
        }
        public IEnumerable<Project> Membership()
        {
            return api.Get().GetAll<Project>(Project.Url + "/?membership=true");
        }
        
        public IEnumerable<Project> Starred() {
            return api.Get().GetAll<Project>(Project.Url + "/?starred=true");
        }

        public Project Get(int id) {
            return api.Get().To<Project>(Project.Url + "/" + id);
        }


        public Project Get(string namespacedpath)
        {
            return api.Get().To<Project>(Project.Url + "/" +  namespacedpath.Replace("/", "%2F"));
        }


        public Project Create(ProjectCreate project) {
            return api.Post().With(project).To<Project>(Project.Url);
        }

        public bool Delete(int id) {
            return api.Delete().To<Project>(Project.Url + "/" + id).Id == default(int);
        }

        public Project Star(int id) {
            return api.Post().To<Project>(Project.Url + "/" + id + "/star");
        }
    }
}