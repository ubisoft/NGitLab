using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class FileClient : IFilesClient
    {
        private readonly API _api;
        private readonly string _repoPath;

        public FileClient(API api, string repoPath)
        {
            _api = api;
            _repoPath = repoPath;
        }

        public IEnumerable<FileData> All
        {
            get { return _api.Get().GetAll<FileData>(_repoPath + "/files"); } 
        }

        public void Create(FileUpsert file)
        {
            _api.Post().With(file).Stream(_repoPath + "/files", s => { });
        }

        public void Update(FileUpsert file)
        {
            _api.Put().With(file).Stream(_repoPath + "/files", s => { });
        }

        public void Delete(FileDelete file)
        {
            _api.Delete().With(file).Stream(_repoPath + "/files", s => { });
        }
    }
}