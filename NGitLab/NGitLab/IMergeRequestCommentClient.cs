using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab {
    public interface IMergeRequestCommentClient {
        IEnumerable<Comment> All();

        Comment Add(MergeRequestComment comment);
    }
}