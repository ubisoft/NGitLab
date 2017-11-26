using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Impl {
    public class RepositoryClient : IRepositoryClient {
        readonly Api api;
        readonly string projectPath;
        readonly string repoPath;

        public RepositoryClient(Api api, int projectId) {
            this.api = api;

            projectPath = Project.Url + "/" + projectId;
            repoPath = projectPath + "/repository";
        }

        public IEnumerable<Tag> Tags => api.Get().GetAll<Tag>(repoPath + "/tags");

        public IEnumerable<TreeOrBlob> Tree => api.Get().GetAll<TreeOrBlob>(repoPath + "/tree");

        public void GetRawBlob(string sha, Action<Stream> parser) {
            api.Get().Stream(repoPath + "/raw_blobs/" + sha, parser);
        }

        public IEnumerable<Commit> Commits => api.Get().GetAll<Commit>(repoPath + "/commits");

        public SingleCommit GetCommit(Sha1 sha) {
            return api.Get().To<SingleCommit>(repoPath + "/commits/" + sha);
        }

        public IEnumerable<Diff> GetCommitDiff(Sha1 sha) {
            return api.Get().GetAll<Diff>(repoPath + "/commits/" + sha + "/diff");
        }
        public CompareInfo Compare(Sha1 from, Sha1 to) {
            return api.Get().To<CompareInfo>(repoPath + $@"/compare?from={from}&to={to}");
        }
        public IFilesClient Files => new FileClient(api, repoPath);

        public IBranchClient Branches => new BranchClient(api, repoPath);

        public IPipelinesClient Pipelines => new PipelinesClient(api, projectPath);

        public IJobsClient Jobs => new JobsClient(api, projectPath);
        
        public IProjectHooksClient ProjectHooks => new ProjectHooksClient(api, projectPath);

        public IProjectSnippetsClient ProjectSnippets => new  ProjectSnippetsClient (api, projectPath);
        public Tag CreateTag(TagCreate tag) {
            return api
                .Post().With(tag)
                .To<Tag>(repoPath + "/tags");
        }

        public bool DeleteTag(string tagName) {
            var tag = api.Delete().To<Tag>(repoPath + "/tags/" + tagName);
            return tag == null || tag.Commit == null;
        }

        public CommitStatus GetCommitStatus(Sha1 sha) {
            return api.Get().GetAll<CommitStatus>(repoPath + "/commits/" + sha + "/statuses").FirstOrDefault();
        }
    }
}