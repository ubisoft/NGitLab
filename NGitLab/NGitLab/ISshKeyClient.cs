using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NGitLab.Models;

namespace NGitLab
{
    public interface ISshKeyClient
    {
        IEnumerable<SshKey> All { get; }

        SshKey this[int keyId] { get; }

        SshKey Add(SshKeyCreate key);

        SshKey Remove(int keyId);
    }
}