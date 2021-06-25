using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface ITagClient
    {
        Tag Create(TagCreate tag);

        void Delete(string name);

        ReleaseInfo CreateRelease(string name, ReleaseCreate data);

        ReleaseInfo UpdateRelease(string name, ReleaseUpdate data);

        IEnumerable<Tag> All { get; }
    }
}
