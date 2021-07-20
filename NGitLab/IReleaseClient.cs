using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IReleaseClient
    {
        IEnumerable<ReleaseInfo> All { get; }

        ReleaseInfo this[string tagName] { get; }

        ReleaseInfo Create(ReleaseCreate data);

        ReleaseInfo Update(ReleaseUpdate data);

        void Delete(string tagName);

        IReleaseLinkClient ReleaseLinks(string tagName);
    }
}
