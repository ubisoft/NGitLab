using System;
using System.Collections.Generic;
using System.IO;
using NGitLab.Models;

namespace NGitLab
{
    public interface IRepositoryClient
    {
        IEnumerable<Tag> Tags { get; }
        IEnumerable<TreeOrBlob> Tree { get; }
        void GetRawBlob(string sha, Action<Stream> parser);
        
        IEnumerable<Commit> Commits { get; }
        SingleCommit GetCommit(Sha1 sha);
        IEnumerable<Diff> GetCommitDiff(string sha);

        IFilesClient Files { get; }

        IBranchClient Branches { get; }
    }
}