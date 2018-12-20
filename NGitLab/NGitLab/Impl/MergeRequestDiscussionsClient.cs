using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class MergeRequestDiscussionsClient : IMergeRequestDiscussionsClient
    {
        private readonly API _api;
        private readonly string _discussionsPath;

        public MergeRequestDiscussionsClient(API api, string projectPath, int mergeRequestIid)
        {
            _api = api;
            _discussionsPath = projectPath + "/merge_requests/" + mergeRequestIid + "/discussions";
        }

        public IEnumerable<MergeRequestDiscussion> All => _api.Get().GetAll<MergeRequestDiscussion>(_discussionsPath);
    }
}