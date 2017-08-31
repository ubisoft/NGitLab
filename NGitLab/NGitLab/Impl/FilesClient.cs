using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
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
            return _api.Get().To<FileData>(_repoPath + $"/files/{EncodeFilePath(filePath)}?ref={@ref}");
        }

        private static string EncodeFilePath(string path)
        {
            return Uri.EscapeUriString(path).Replace(".", "%2e");
        }

        private static string SerializeUrlEncoded(object obj)
        {
            if (obj == null)
                return null;

            IDictionary<string, object> dict = new Dictionary<string, object>();
            var fields = obj.GetType().GetFields();
            foreach (var field in fields)
            {
                var attribute = field.GetCustomAttributes(typeof(DataMemberAttribute), true).OfType<DataMemberAttribute>().FirstOrDefault();
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