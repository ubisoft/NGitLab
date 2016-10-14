using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IMergeRequestCommentClient
    {
        IEnumerable<MergeRequestComment> All { get; }

        MergeRequestComment Add(MergeRequestComment comment);
    }
}