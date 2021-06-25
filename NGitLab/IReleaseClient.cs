using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IReleaseClient
    {
        ReleaseInfo Create(ReleaseCreate release);
        ReleaseInfo Update(ReleaseUpdate release);

        void Delete(string name);

        IEnumerable<ReleaseInfo> All { get; }
    }
}
