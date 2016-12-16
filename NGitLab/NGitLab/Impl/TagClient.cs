using System.Collections;
using System.Collections.Generic;
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

        public Tag Create(TagUpsert tag)
        {
            return _api.Post().With(tag).To<Tag>(_tagsPath);
        }

        public Tag Update(TagUpsert tag)
        {
            return _api.Put().With(tag).To<Tag>(_tagsPath);
        }

        public void Delete(string name)
        {
            _api.Delete().To<Tag>(_tagsPath);
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