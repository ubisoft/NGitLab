using System.Web;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class FilesClient : IFilesClient
    {
        private readonly API _api;
        private readonly string _repoPath;

        public FilesClient(API api, string repoPath)
        {
            _api = api;
            _repoPath = repoPath;
        }

        public void Create(FileUpsert file)
        {
            _api.Post().With(file).Stream(_repoPath + $"/files?file_path={HttpUtility.UrlEncode(file.Path)}&branch_name={file.Branch}", s => { });
        }

        public void Update(FileUpsert file)
        {
            _api.Put().With(file).Stream(_repoPath + "/files", s => { });
        }

        public void Delete(FileDelete file)
        {
            _api.Delete().With(file).Stream(_repoPath + "/files", s => { });
        }

        public FileData Get(string filePath, string @ref)
        {
            return _api.Get().To<FileData>(_repoPath + $"/files?file_path={HttpUtility.UrlEncode(filePath)}&ref={@ref}");
        }
    }
}