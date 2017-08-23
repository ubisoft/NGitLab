using NGitLab.Models;

namespace NGitLab.Impl {
    public class FileClient : IFilesClient {
        readonly Api api;
        readonly string repoPath;

        public FileClient(Api api, string repoPath) {
            this.api = api;
            this.repoPath = repoPath;
        }

        public void Create(FileUpsert file) {
            api.Post().With(file).Stream(repoPath + "/files", s => { });
        }

        public void Update(FileUpsert file) {
            api.Put().With(file).Stream(repoPath + "/files", s => { });
        }

        public void Delete(FileDelete file) {
            api.Delete().With(file).Stream(repoPath + "/files", s => { });
        }
    }
}