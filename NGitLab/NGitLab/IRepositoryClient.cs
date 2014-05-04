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
        SingleCommit GetCommit(string sha);
        IEnumerable<Diff> GetCommitDiff(string sha);

        IEnumerable<FileData> Files { get; }
        void CreateFile(FileUpsert file);
        void UpdateFile(FileUpsert file);
        void DeleteFile(FileDelete file);

        IEnumerable<Branch> Branches { get; }
        Branch GetBranch(string name);
        Branch ProtectBranch(string name);
        Branch UnprotectBranch(string name);
        Branch Create(BranchCreate branch);
    }
}