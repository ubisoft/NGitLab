using System.Collections;
using System.Collections.Generic;
using System.Web;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class TagClient : ITagClient
    {
        private readonly API _api;
        private readonly string _tagsPath;

        public TagClient(API api, string repoPath)
        {
            _api = api;
            _tagsPath = repoPath + "/tags";
        }

        public Tag Create(TagCreate tag)
        {
            return _api.Post().With(tag).To<Tag>(_tagsPath);
        }

        public void Delete(string name)
        {
            _api.Delete().Stream($"{_tagsPath}/{HttpUtility.UrlEncode(name)}", x => {});
        }

        public IEnumerator<Tag> GetEnumerator()
        {
            return _api.Get().GetAll<Tag>(_tagsPath).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}