using System;
using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IMergeRequestCommentClient
    {
        IEnumerable<MergeRequestComment> All { get; }

        [Obsolete("Use Add(MergeRequestCommentCreate comment)")]
        MergeRequestComment Add(MergeRequestComment comment);

        MergeRequestComment Add(MergeRequestCommentCreate comment);

        MergeRequestComment Edit(long id, MergeRequestCommentEdit comment);
    }
}
