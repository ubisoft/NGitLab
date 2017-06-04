using System;
using System.Collections.Generic;
using System.IO;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class RepositoryClient : IRepositoryClient
    {
        private readonly API _api;
        private readonly string _repoPath;
        private readonly string _projectPath;

        public RepositoryClient(API api, int projectId)
        {
            _api = api;

            _projectPath = Project.Url + "/" + projectId;
            _repoPath = _projectPath + "/repository";
        }

        public IEnumerable<Tag> Tags
        {
            get { return _api.Get().GetAll<Tag>(_repoPath + "/tags"); }
        }

        public IEnumerable<TreeOrBlob> Tree
        {
            get { return _api.Get().GetAll<TreeOrBlob>(_repoPath + "/tree"); }
        }

        public void GetRawBlob(string sha, Action<Stream> parser)
        {
            _api.Get().Stream(_repoPath + "/raw_blobs/" + sha, parser);
        }

        public IEnumerable<Commit> Commits
        {
            get { return _api.Get().GetAll<Commit>(_repoPath + "/commits"); }
        }

        public SingleCommit GetCommit(Sha1 sha)
        {
            return _api.Get().To<SingleCommit>(_repoPath + "/commits/" + sha);
        }

        public IEnumerable<Diff> GetCommitDiff(Sha1 sha)
        {
            return _api.Get().GetAll<Diff>(_repoPath + "/commits/" + sha + "/diff");
        }

        public IFilesClient Files
        {
            get { return new FileClient(_api, _repoPath); }
        }

        public IBranchClient Branches
        {
            get { return new BranchClient(_api, _repoPath); }
        }

        public IProjectHooksClient ProjectHooks
        {
            get { return new ProjectHooksClient(_api, _projectPath); }
        }

        public Tag CreateTag(TagCreate tag)
        {
            return _api
                .Post().With(tag)
                .To<Tag>(_repoPath + "/tags");
        }

        public bool DeleteTag(String tagName)
        {
            Tag tag = _api.Delete().To<Tag>(_repoPath + "/tags/" + tagName);
            return (tag == null || tag.Commit == null);
        }
    }
}