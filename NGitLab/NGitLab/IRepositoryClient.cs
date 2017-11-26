using System;
using System.Collections.Generic;
using System.IO;
using NGitLab.Models;

namespace NGitLab {
    public interface IRepositoryClient {
        IEnumerable<Tag> Tags { get; }
        IEnumerable<TreeOrBlob> Tree { get; }
        IEnumerable<Commit> Commits { get; }
        IFilesClient Files { get; }
        IBranchClient Branches { get; }
        IPipelinesClient Pipelines { get; }
        IJobsClient Jobs { get; }
        IProjectHooksClient ProjectHooks { get; }
        IProjectSnippetsClient ProjectSnippets { get; }
        void GetRawBlob(string sha, Action<Stream> parser);
        SingleCommit GetCommit(Sha1 sha);
        IEnumerable<Diff> GetCommitDiff(Sha1 sha);
        CompareInfo Compare(Sha1 from, Sha1 to);
        CommitStatus GetCommitStatus(Sha1 sha);
        Tag CreateTag(TagCreate tag);
        bool DeleteTag(string tagName);
    }
}