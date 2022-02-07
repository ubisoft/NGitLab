using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IContributorClient
    {
        IEnumerable<Contributor> All { get; }
    }
}
