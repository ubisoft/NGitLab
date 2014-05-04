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

        public SingleCommit GetCommit(string sha)
        {
            return _api.Get().To<SingleCommit>(_repoPath + "/commits/" + sha);
        }

        public IEnumerable<Diff> GetCommitDiff(string sha)
        {
            return _api.Get().GetAll<Diff>(_repoPath + "/commits/" + sha + "/diff");
        }

        public IEnumerable<FileData> Files
        {
            get { return _api.Get().GetAll<FileData>(_repoPath + "/files"); }
        }

        public void CreateFile(FileUpsert file)
        {
            _api.Post().With(file).Stream(_repoPath + "/files", s => { });
        }

        public void UpdateFile(FileUpsert file)
        {
            _api.Put().With(file).Stream(_repoPath + "/files", s => { });
        }

        public void DeleteFile(FileDelete file)
        {
            _api.Delete().With(file).Stream(_repoPath + "/files", s => { });
        }

        public IEnumerable<Branch> Branches
        {
            get { return _api.Get().GetAll<Branch>(_repoPath + "/branches"); }
        }

        public Branch GetBranch(string name)
        {
            return _api.Get().To<Branch>(_repoPath + "/branches/" + name);
        }

        public Branch ProtectBranch(string name)
        {
            return _api.Put().To<Branch>(_repoPath + "/branches/" + name + "/protect");
        }

        public Branch UnprotectBranch(string name)
        {
            return _api.Put().To<Branch>(_repoPath + "/branches/" + name + "/unprotect");
        }

        public Branch Create(BranchCreate branch)
        {
            return _api.Post().With(branch).To<Branch>(_repoPath + "/branches");
        }
    }
}