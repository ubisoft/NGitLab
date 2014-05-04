using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class RepositoryClient : IRepositoryClient
    {
        private readonly API _api;
        private readonly string _repoPath;

        public RepositoryClient(API api, int projectId)
        {
            _api = api;

            _repoPath = Project.Url + "/" + projectId + "/repository";
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

        public IEnumerable<Diff> GetCommitDiff(string sha)
        {
            return _api.Get().GetAll<Diff>(_repoPath + "/commits/" + sha + "/diff");
        }

        public IFilesClient Files
        {
            get{return new FileClient(_api, _repoPath);}
        }


        public IBranchClient Branches
        {
            get { return new BranchClient(_api, _repoPath); }
        }
    }
}