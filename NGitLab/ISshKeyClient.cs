using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab;

public interface ISshKeyClient
{
    IEnumerable<SshKey> All { get; }

    SshKey this[long keyId] { get; }

    SshKey Add(SshKeyCreate key);

    void Remove(long keyId);
}
