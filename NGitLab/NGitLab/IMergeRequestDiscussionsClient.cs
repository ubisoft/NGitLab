using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IMergeRequestDiscussionsClient
    {
        IEnumerable<MergeRequestDiscussion> All { get; }
    }
}