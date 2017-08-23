using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl {
    public class ProjectClient : IProjectClient {
        readonly API _api;

        public ProjectClient(API api) {
            _api = api;
        }

        public IEnumerable<Project> Accessible() {
            return _api.Get().GetAll<Project>(Project.Url);
        }

        public IEnumerable<Project> Owned() {
            return _api.Get().GetAll<Project>(Project.Url + "/?owned=true");
        }

        public IEnumerable<Project> Starred() {
            return _api.Get().GetAll<Project>(Project.Url + "/?starred=true");
        }

        public Project Get(int id) {
            return _api.Get().To<Project>(Project.Url + "/" + id);
        }

        public Project Create(ProjectCreate project) {
            return _api.Post().With(project).To<Project>(Project.Url);
        }

        public bool Delete(int id) {
            return _api.Delete().To<Project>(Project.Url + "/" + id).Id == default(int);
        }

        public Project Star(int id) {
            return _api.Post().To<Project>(Project.Url + "/" + id + "/star");
        }
    }
}