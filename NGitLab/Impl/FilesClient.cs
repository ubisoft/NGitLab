using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
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
            var url = QueryStringHelper.BuildAndAppendQueryString($"{_repoPath}/files/{EncodeFilePath(file.Path)}", file);
            _api.Post().With(file).Execute(url);
        }

        public void Update(FileUpsert file)
        {
            var url = QueryStringHelper.BuildAndAppendQueryString($"{_repoPath}/files/{EncodeFilePath(file.Path)}", file);
            _api.Put().With(file).Execute(url);
        }

        public void Delete(FileDelete file)
        {
            var url = QueryStringHelper.BuildAndAppendQueryString($"{_repoPath}/files/{EncodeFilePath(file.Path)}", file);
            _api.Delete().Execute(url);
        }

        public FileData Get(string filePath, string @ref)
        {
            return _api.Get().To<FileData>(_repoPath + $"/files/{EncodeFilePath(filePath)}?ref={Uri.EscapeDataString(@ref)}");
        }

        public Task<FileData> GetAsync(string filePath, string @ref, CancellationToken cancellationToken = default)
        {
            return _api.Get().ToAsync<FileData>(_repoPath + $"/files/{EncodeFilePath(filePath)}?ref={Uri.EscapeDataString(@ref)}", cancellationToken);
        }

        public bool FileExists(string filePath, string @ref)
        {
            try
            {
                _api.Head().Execute(_repoPath + $"/files/{EncodeFilePath(filePath)}?ref={@ref}");
                return true;
            }
            catch (GitLabException e) when (e.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
        }

        public Blame[] Blame(string filePath, string @ref)
        {
            return _api.Get().To<Blame[]>(_repoPath + $"/files/{EncodeFilePath(filePath)}/blame?ref={@ref}");
        }

        private static string EncodeFilePath(string path)
        {
            return Uri.EscapeDataString(path);
        }
    }
}
