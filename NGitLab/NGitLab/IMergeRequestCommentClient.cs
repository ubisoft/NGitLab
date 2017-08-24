using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab {
    public interface IMergeRequestCommentClient {
        IEnumerable<Comment> All { get; }

        Comment Add(MergeRequestComment comment);
    }
}