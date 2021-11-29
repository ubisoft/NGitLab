using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
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
            _api.Post().With(file).Execute(_repoPath + $"/files/{EncodeFilePath(file.Path)}?{SerializeUrlEncoded(file)}");
        }

        public void Update(FileUpsert file)
        {
            _api.Put().With(file).Execute(_repoPath + $"/files/{EncodeFilePath(file.Path)}?{SerializeUrlEncoded(file)}");
        }

        public void Delete(FileDelete file)
        {
            _api.Delete().Execute(_repoPath + $"/files/{EncodeFilePath(file.Path)}?{SerializeUrlEncoded(file)}");
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

        private static string SerializeUrlEncoded(object obj)
        {
            if (obj == null)
                return null;

            var dict = new Dictionary<string, object>(StringComparer.Ordinal);
            var fields = obj.GetType().GetFields();
            foreach (var field in fields)
            {
                var attribute = field.GetCustomAttributes(typeof(DataMemberAttribute), inherit: true).OfType<DataMemberAttribute>().FirstOrDefault();
                if (attribute == null)
                    continue;

                dict[attribute.Name ?? field.Name] = field.GetValue(obj);
            }

            return SerializeUrlEncoded(dict);
        }

        private static string SerializeUrlEncoded(IDictionary<string, object> dict)
        {
            if (dict == null)
                return null;

            var sb = new StringBuilder();
            foreach (var kvp in dict)
            {
                if (kvp.Value == null)
                    continue;

                if (sb.Length > 0)
                {
                    sb.Append('&');
                }

                sb.Append(kvp.Key);
                sb.Append('=');
                sb.Append(Uri.EscapeDataString($"{kvp.Value}"));
            }

            return sb.ToString();
        }
    }
}
