using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab;

public interface ICommitStatusClient
{
    IEnumerable<CommitStatus> AllBySha(string commitSha);

    CommitStatusCreate AddOrUpdate(CommitStatusCreate status);
}
